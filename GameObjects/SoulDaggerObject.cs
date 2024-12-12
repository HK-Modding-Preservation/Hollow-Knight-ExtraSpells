using GlobalEnums;
using Modding.Utils;
using System.Collections;

namespace ExtraSpells.GameObjects
{
    public class SoulDaggerObject : MonoBehaviour
    {
        float damagenumber = 10;
        float speed = 35f;
        private Rigidbody2D _rb;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            Destroy(GetComponent<DamageHero>());
            Destroy(GetComponent<AutoRecycleSelf>());

            Destroy(GetComponent<FaceAngleSimple>());
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            HealthManager manager = col.gameObject.GetComponent<HealthManager>();
            if (manager == null) {
                //edge case for pv, i guess
                manager = col.GetComponentInParent<HealthManager>(); 
            }

            if (manager == null) { return; }

            float multiplier = 1f;

            if (HeroController.instance.playerData.equippedCharm_19)
            {
                transform.localScale += new Vector3((float)0.3, (float)0.3);
                multiplier *= 1.25f;
            }

            if (HeroController.instance.playerData.fireballLevel == 2)
            {
                damagenumber = 12;
            }

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
                Multiplier = multiplier,
                Source = gameObject,
                SpecialType = SpecialTypes.None
            });
        }

        public void Start()
        {
            _rb.isKinematic = false;

            gameObject.layer = (int)PhysLayers.HERO_ATTACK;
        }

        private IEnumerator DestroyAfter()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        public void Update()
        {
            Vector2 direction = transform.up;
            _rb.velocity = direction * speed;
        }
    }
}
