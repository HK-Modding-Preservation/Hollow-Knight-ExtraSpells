using SpellChanger.AbilityClasses;
using SpellChanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSpells.Spells
{
    //Nail art, this time. Works the same, just only need one sprite.
    //This one was the first test, and I just wanna try using dash slash without the unwieldy dash
    public class FlashDraw
    {
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.flashdraw.png");
            Sprite[] sprites = new Sprite[] { sprite1 };
            CustomGreatSlash flashDraw = new CustomGreatSlash("FlashDraw", "INV_NAME_ART_FLASHDRAW", "INV_DESC_ART_FLASHDRAW", sprites);

            FsmState DSlashStartState = flashDraw.CopyStateActions("DSlash Start", "DSlash Start");
            FsmState FacingState = flashDraw.CopyStateActions("Facing? 2", "Facing? 2");
            FsmState Left2State = flashDraw.CopyStateActions("Left 2", "Left 2");
            FsmState Right2State = flashDraw.CopyStateActions("Right 2", "Right 2");
            FsmState DashSlashState = flashDraw.CopyStateActions("Dash Slash", "Dash Slash");
            FsmState DSlashMoveEndState = flashDraw.CopyStateActions("DSlash Move End", "DSlash Move End");
            FsmState DSlashEndState = flashDraw.CopyStateActions("D Slash End", "D Slash End");

            flashDraw.AddTransition("FINISHED", "Facing? 2", "DSlash Start");
            flashDraw.AddTransition("LEFT", "Left 2", "Facing? 2");
            flashDraw.AddTransition("RIGHT", "Right 2", "Facing? 2");
            flashDraw.AddTransition("FINISHED", "Dash Slash", "Left 2");
            flashDraw.AddTransition("FINISHED", "Dash Slash", "Right 2");
            flashDraw.AddTransition("FINISHED", "DSlash Move End", "Dash Slash");
            flashDraw.AddTransition("FINISHED", "D Slash End", "DSlash Move End");



            SpellHelper.AddSpell(flashDraw, true);
        }
    }
}
