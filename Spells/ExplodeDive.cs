using ExtraSpells.GameObjects;
using SpellChanger;
using SpellChanger.AbilityClasses;

namespace ExtraSpells.Spells
{
    public static class ExplodeDive
    {
        //An example of a spell made almost entirely from a copy of an existing spell.
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.explodedive1.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite1 };
            CustomQuake explodeDive = new CustomQuake("ExplodeDive", "INV_NAME_SPELL_EXPLODEDIVE", "INV_DESC_SPELL_EXPLODEDIVE", sprites);

            PlayMakerFSM spellControl = explodeDive.storedFSM;
            FsmOwnerDefault ownerDefault = Helper.GetKnightOwnerDefault();
            if (ownerDefault == null) { return; }

            //cloning states from dive to work with
            //because customspells have a unique id automatically added to state names,
            //you can use identical names to one another no problem
            //but always remember the custom spell states DO NOT AND SHOULD NOT interact with base spells.
            //this is to ensure maximum compatibility between mods
            FsmState OnGroundState = explodeDive.CopyStateActions("On Ground?", "On Ground?");
            FsmState QOnGroundState = explodeDive.CopyStateActions("Q On Ground", "Q On Ground");
            FsmState QOffGroundState = explodeDive.CopyStateActions("Q Off Ground", "Q Off Ground");
            FsmState QuakeAnticState = explodeDive.CopyStateActions("Quake Antic", "Quake Antic");
            FsmState Q1EffectState = explodeDive.CopyStateActions("Q1 Effect", "Q1 Effect");
            FsmState QuakeDownState = explodeDive.CopyStateActions("Quake1 Down", "Quake1 Down");
            FsmState QuakeLandState = explodeDive.CopyStateActions("Quake1 Land", "Quake1 Land");
            FsmState QuakeFinishState = explodeDive.CopyStateActions("Quake Finish", "Quake Finish");

            QuakeLandState.RemoveAction(12); //activate slam
            QuakeLandState.RemoveAction(9); //activate pillar effect
            QuakeLandState.RemoveAction(8); //activate flash slam
            QuakeLandState.InsertMethod(8,() =>
            {
                GameObject explosion = UnityEngine.Object.Instantiate(ResourceLoader.explosionObject);
                explosion.transform.position = HeroController.instance.transform.position;

                explosion.AddComponent<ModifyExplosion>();
            });

            //on ground
            explodeDive.AddTransition("ON GROUND", "Q On Ground", "On Ground?");
            explodeDive.AddTransition("OFF GROUND", "Q Off Ground", "On Ground?");
            //q on ground
            explodeDive.AddTransition("FINISHED", "Quake Antic", "Q On Ground");
            //q off ground
            explodeDive.AddTransition("FINISHED", "Quake Antic", "Q Off Ground");
            //quake antic
            explodeDive.AddTransition("ANIM END", "Q1 Effect", "Quake Antic");
            //q1 effect
            explodeDive.AddTransition("FINISHED", "Quake1 Down", "Q1 Effect");
            //quake1 down
            explodeDive.AddTransition("HERO LANDED", "Quake1 Land", "Quake1 Down");
            //quake1 land
            explodeDive.AddTransition("FINISHED", "Quake Finish", "Quake1 Land");

            explodeDive.mpCostState = Q1EffectState;
            SpellHelper.AddSpell(explodeDive, true);
        }
    }
}
