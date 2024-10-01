using GlobalEnums;
using Modding.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSpells.GameObjects
{
    public class TendrilObject :MonoBehaviour
    {
        GameObject tendril1;
        GameObject tendril2;
        GameObject hitbox;
        PolygonCollider2D collider;
        float damagenumber = 15;

        public void Awake()
        {
            hitbox = gameObject.Child("T Hit");
            tendril1 = gameObject.Child("T1");
            tendril2 = gameObject.Child("T2");
            collider = hitbox.GetComponent<PolygonCollider2D>();

            Destroy(GetComponent<DamageHero>());

            
        }

        public void Start()
        {
            tendril1.SetActive(true);
            tendril2.SetActive(true);
            hitbox.SetActive(true);

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                transform.localScale += new Vector3((float)-0.1, (float)0);
                damagenumber = 30;
            }

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)-0.1, (float)0);
                damagenumber += 10;
            }

            GameManager.instance.StartCoroutine(Hitboxes());
        }

        private void HitActivate()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;

            List<Collider2D> results = new List<Collider2D> ();

            collider.OverlapCollider(filter, results);

            foreach (Collider2D col in results)
            {
                HealthManager manager = col.gameObject.GetComponent<HealthManager>();
                if (manager == null) { continue; }


                manager.Hit(new HitInstance
                {
                    AttackType = AttackTypes.Spell,
                    CircleDirection = false,
                    DamageDealt = (int)damagenumber,
                    MagnitudeMultiplier = 0,
                    Direction = 0,
                    IgnoreInvulnerable = true,
                    IsExtraDamage = false,
                    MoveAngle = 0,
                    MoveDirection = false,
                    Multiplier = 1,
                    Source = gameObject,
                    SpecialType = SpecialTypes.None
            });
            }
        }

        private IEnumerator Hitboxes()
        {
            HitActivate();
            HeroController.instance.gameObject.Child("Attacks").Child("Slash").GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.15f);
            HitActivate();
            HeroController.instance.gameObject.Child("Attacks").Child("Slash").GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
        }



    }
}

