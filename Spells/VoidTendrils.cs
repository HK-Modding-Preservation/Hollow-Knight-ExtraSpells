using SpellChanger.AbilityClasses;
using SpellChanger;
using HutongGames.PlayMaker.Actions;
using ExtraSpells.GameObjects;

namespace ExtraSpells.Spells
{
    public class VoidTendrils
    {
        //An example of an attack with designated hitboxes - similar to the scream.
        //Also this one is just cool my bad
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.voidtendrils.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite1 };
            CustomFireball voidTendrils = new CustomFireball("VoidTendrils", "INV_NAME_SPELL_VOIDTENDRILS", "INV_DESC_SPELL_VOIDTENDRILS", sprites);

            PlayMakerFSM spellControl = voidTendrils.storedFSM;
            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();
            if (ownerDefault == null) { return; }

            //Antic state
            FsmState AnticState = voidTendrils.CreateState("Void Tendril Antic");
            AnticState.AddMethod(() => {
                Helper.StopKnightControl();
                HeroController.instance.AffectedByGravity(false);
                GameManager.instance.StartCoroutine(Helper.SendEventAfterAnim("ANIM END", "Fireball Antic", spellControl));
            });
            AnticState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });

            //Tendril shoot state
            FsmState TendrilState = voidTendrils.CreateState("Tendrils");
            TendrilState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });
            TendrilState.AddMethod(() =>
            {
                Helper.PlayAnim("Fireball2 Cast");
                //create
                GameObject tendrils = UnityEngine.Object.Instantiate(ResourceLoader.tendrilObject);
                tendrils.AddComponent<TendrilObject>();

                tendrils.transform.parent = HeroController.instance.transform;
                tendrils.transform.SetScaleX(-0.35f);
                tendrils.transform.localPosition = new Vector3(-0.55f, 0.2f);

                tendrils.SetActive(true);
            });
            TendrilState.AddAction(new Wait { realTime = true, finishEvent = FsmEvent.Finished, time = 0.3f });

            //End state
            FsmState EndState = voidTendrils.CreateState("Tendril End");
            EndState.AddMethod(() => {
                Helper.StartKnightControl();
                HeroController.instance.AffectedByGravity(true);
            });

            //transitions
            voidTendrils.AddTransition("ANIM END", "Tendrils", "Void Tendril Antic");
            voidTendrils.AddTransition("FINISHED", "Tendril End", "Tendrils");

            voidTendrils.mpCostState = TendrilState;

            SpellHelper.AddSpell(voidTendrils, true);
        }
    }
}
