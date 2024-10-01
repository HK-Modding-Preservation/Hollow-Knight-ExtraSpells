using GlobalEnums;
using Modding.Utils;
using System.Collections;

namespace ExtraSpells.GameObjects
{
    public class ModifyExplosion : MonoBehaviour
    {
        float damagenumber = 40;

        public void Awake()
        {
            Destroy(GetComponent<DamageHero>());


            PlayMakerFSM damagesEnemy = gameObject.LocateMyFSM("damages_enemy");
            if (damagesEnemy == null) { return; }

            gameObject.layer = (int)PhysLayers.HERO_ATTACK;

            float multiplier = 1f;

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)0.3, (float)0.3);
                multiplier *= 1.25f;
            }

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                transform.localScale += new Vector3((float)0.3, (float)0.3);
                damagenumber = 55;
            }

            damagesEnemy.FsmVariables.GetFsmInt("damageDealt").Value = (int)Math.Ceiling(damagenumber * multiplier);
        }

    }
}
