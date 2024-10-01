using ExtraSpells.GameObjects;
using HutongGames.PlayMaker.Actions;
using SpellChanger;
using SpellChanger.AbilityClasses;
using SpellChanger.Utils;
using System.Collections;

namespace ExtraSpells.Spells
{
    public static class SoulDaggers
    {
        //An example of a hybrid spell - made using both built in FSM actions and custom methods.
        //Also an example of modifying a gameobject to fit your needs.
        //And also using coroutines mid spell.
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.souldagger1.png");
            Sprite sprite2 = ResourceLoader.LoadSprite("ExtraSpells.Resources.souldagger2.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite2 };
            CustomFireball soulDaggers = new CustomFireball("SoulDaggers", "INV_NAME_SPELL_SOULDAGGERS", "INV_DESC_SPELL_SOULDAGGERS", sprites);

            PlayMakerFSM spellControl = soulDaggers.storedFSM; ; 
            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();
            if (ownerDefault == null) { return; }

            //Antic state
            FsmState AnticState = soulDaggers.CreateState("Soul Dagger Antic");
            AnticState.AddMethod(() => {
                Helper.StopKnightControl();
                HeroController.instance.AffectedByGravity(false);
                GameManager.instance.StartCoroutine(Helper.SendEventAfterAnim("ANIM END", "Fireball Antic", spellControl));
            });
            AnticState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });

            //Dagger shoot state
            FsmState DaggerState = soulDaggers.CreateState("Daggers");
            DaggerState.AddAction(new SetVelocity2d { everyFrame = true, gameObject = ownerDefault, vector = new Vector2(), x = 0, y = 0 });
            DaggerState.AddMethod(() =>
            {
                Helper.PlayAnim("Fireball1 Cast");
                for (int i = 1; i < 4; i++)
                {
                    GameManager.instance.StartCoroutine(CreateDagger(i, spellControl, DaggerState.Name));
                }
                if (HeroController.instance.playerData.fireballLevel < 2) { return; }
                for (int i = 4; i < 7; i++)
                {
                    GameManager.instance.StartCoroutine(CreateDagger(i, spellControl, DaggerState.Name));
                }
                //GameManager.instance.StartCoroutine(Helper.SendEventAfterTime("DONE", 0.48f, spellControl));
            });
            DaggerState.AddAction(new Wait { realTime = true, finishEvent = FsmEvent.Finished, time = 0.3f });

            //End state
            FsmState EndState = soulDaggers.CreateState("Dagger End");
            EndState.AddMethod(() => {
                Helper.StartKnightControl();
                HeroController.instance.AffectedByGravity(true);
            });

            //transitions
            soulDaggers.AddTransition("ANIM END", "Daggers", "Soul Dagger Antic");
            soulDaggers.AddTransition("FINISHED", "Dagger End", "Daggers");
   

            SpellHelper.AddSpell(soulDaggers, true);
        }

        private static IEnumerator CreateDagger(int i, PlayMakerFSM spellControl, string EndStateName)
        {
            yield return new WaitForSeconds(0.05f * i);
            if (spellControl.ActiveStateName != EndStateName) { yield break; };

            GameObject clone = UnityEngine.Object.Instantiate(ResourceLoader.soulDagger);
            SoulDaggerObject mono = clone.AddComponent<SoulDaggerObject>();

            bool facingLeft = true;
            if (HeroController.instance.gameObject.transform.GetScaleX() < 0) { facingLeft = false; }

            var value = facingLeft ? 110 : 250;
            float modifier = facingLeft ? -1 : 1;
            //mono.SetAngle(value + 10 * i * modifier);
            clone.transform.rotation = Quaternion.Euler(0, 0, value + 10 * i * modifier);
            clone.transform.position = new Vector3(HeroController.instance.transform.position.x, HeroController.instance.transform.position.y, 0);
            clone.SetActive(true);
        }

    }
}
