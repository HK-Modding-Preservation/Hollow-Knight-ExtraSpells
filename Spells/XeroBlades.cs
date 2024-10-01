using SpellChanger.AbilityClasses;
using SpellChanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtraSpells.GameObjects;

namespace ExtraSpells.Spells
{
    public class XeroBlades
    {
        //An example of a spell with a purely unique MP drain system (taking as much as possible).   
        public static void CreateSpell()
        {
            Sprite sprite1 = ResourceLoader.LoadSprite("ExtraSpells.Resources.xeroblades.png");
            Sprite[] sprites = new Sprite[] { sprite1, sprite1 };
            CustomScream xeroBlades = new CustomScream("XeroBlades", "INV_NAME_SPELL_XEROBLADES", "INV_DESC_SPELL_XEROBLADES", sprites);

            FsmState SummonState = xeroBlades.CreateState("Blade Summon");
            SummonState.AddMethod(() => {
                int mp = HeroController.instance.playerData.MPCharge;
                int cost = 33;
                if (HeroController.instance.playerData.equippedCharm_33)
                {
                    cost = 24;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (mp - cost < 0) { continue; }

                    HeroController.instance.TakeMP(cost);

                    CreateBlade(i);

                    mp -= cost;
                }
            });

            xeroBlades.mpCost = 0;

            SpellHelper.AddSpell(xeroBlades, true);
        }

        private static void CreateBlade(int number)
        {
            GameObject blade = UnityEngine.Object.Instantiate(ResourceLoader.xerobladeObject);
            blade.transform.position = HeroController.instance.transform.position;
            XeroBladeObject obj = blade.AddComponent<XeroBladeObject>();
            blade.SetActive(true);
            obj.setBladeNumber(number);
        }
    }
}
