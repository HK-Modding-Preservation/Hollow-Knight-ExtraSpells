using SpellChanger;
using SpellChanger.AbilityClasses;

namespace ExtraSpells.Spells
{
    public static class ShadeSummon
    {
        //An example of a purely method based spell.
        //No inbuild FSM actions.
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.shadesummon1.png");
            Sprite sprite2 = ResourceLoader.LoadSprite("ExtraSpells.Resources.shadesummon2.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite2 };
            CustomScream shadeSummon = new CustomScream("ShadeSummon", "INV_NAME_SPELL_SHADESUMMON", "INV_DESC_SPELL_SHADESUMMON", sprites);

            FsmState SummonState = shadeSummon.CreateState("Shade Summon");
            SummonState.AddMethod(() => {
                GameObject shade = UnityEngine.Object.Instantiate(ResourceLoader.shadeenemy);
                shade.transform.position = HeroController.instance.transform.position + new Vector3(0f, 3f);
                shade.SetActive(true);
            });

            SpellHelper.AddSpell(shadeSummon, true);
        }
    }
}
