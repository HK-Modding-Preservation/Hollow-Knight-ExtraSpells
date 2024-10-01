using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using Modding.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;

namespace ExtraSpells.GameObjects
{
    public class XeroBladeObject : MonoBehaviour
    {
        private PlayMakerFSM fsm;
        private float damagenumber = 30;

        private Vector3[] offsets = { new Vector3(-2, 1), new Vector3(-1, 2), new Vector3(1, 2), new Vector3(2, 1) };
        private int i;
        private DamageEnemies dmg;
        public void Awake()
        {
            fsm = gameObject.GetComponent<PlayMakerFSM>();

            Destroy(gameObject.GetComponent<DamageHero>());

            gameObject.layer = (int)PhysLayers.HERO_ATTACK;
            gameObject.name = "HeroXeroBlade";

            dmg = gameObject.GetOrAddComponent<DamageEnemies>();
            dmg.damageDealt = 15;
            dmg.attackType = AttackTypes.Spell;
            dmg.ignoreInvuln = false;
            dmg.magnitudeMult = 0f;
            dmg.moveDirection = false;
            dmg.circleDirection = false;
            dmg.direction = 0;
            dmg.enabled = true;
            dmg.specialType = SpecialTypes.None;

            float multiplier = 1f;

            if (HeroController.instance.playerData.screamLevel == 2)
            {
                damagenumber = 50;
            }

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                multiplier *= 1.25f;
            }

            dmg.damageDealt = (int)Math.Ceiling(damagenumber * multiplier);

            dmg.enabled = false;
        }

        public void setBladeNumber(int i)
        {
            this.i = i;
            GameManager.instance.StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            GameObject target = getTarget();
            if (target == null) { GameManager.instance.StartCoroutine(Dissipate()); }

            DistanceFlySmooth homeaction = fsm.GetState("Home").GetAction<DistanceFlySmooth>();
            homeaction.target = HeroController.instance.gameObject;
            homeaction.targetRadius = 0.1f;
            homeaction.offset = offsets[i];

            GetAngleToTarget2D action = fsm.GetState("Antic Point").GetAction<GetAngleToTarget2D>();
            action.target = target;
            action.offsetY = 0;

            GetAngleToTarget2D action2 = fsm.GetState("Antic Spin").GetAction<GetAngleToTarget2D>();
            action2.target = target;
            action2.offsetY = 0;

            fsm.GetState("Shoot").RemoveAction(5);
            yield return new WaitForSeconds(0.5f * i);
            fsm.SendEvent("ATTACK");
            dmg.enabled = true;
            yield return new WaitForSeconds(0.9f);
            GameManager.instance.StartCoroutine(Dissipate());
        }

        private IEnumerator Dissipate()
        {
            yield return new WaitForSeconds(0.6f);
            fsm.SetState("Dissipate");
            yield return new WaitForSeconds(0.25f);
            Destroy(gameObject);
        }

        private GameObject getTarget()
        {
            //find a target....
            HealthManager[] obj = FindObjectsOfType<HealthManager>();
            GameObject target = null;
            float distance = 9999999f;
            for (int j = 0; j < obj.Length; j++)
            {

                float mag = (obj[j].gameObject.transform.position - gameObject.transform.position).magnitude;
                if (mag < distance)
                {
                    distance = mag;
                    target = obj[j].gameObject;
                }
            }

            return target;
        }
    }
}

