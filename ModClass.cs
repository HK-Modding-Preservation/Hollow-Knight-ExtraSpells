using SpellChanger;
using ExtraSpells.Spells;


namespace ExtraSpells
{
    public class ExtraSpells : Mod
    {
        internal static ExtraSpells Instance;

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Abyss_15", "Shade Sibling (32)"),
                ("GG_Hollow_Knight","Battle Scene/HK Prime"),
                ("GG_Grey_Prince_Zote","Zote Balloon"),
                ("GG_Ghost_Xero","Warrior/Ghost Warrior Xero")
            };
        }


        public ExtraSpells() : base("ExtraSpells")
        {
           Instance = this;
        }

        public override string GetVersion() => "1.0.0.1";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            ResourceLoader.Initialise(preloadedObjects);
            Events.OnSpellControlLoad += AddSpells;
            Events.OnNailArtControlLoad += AddNailArts;
            ModHooks.LanguageGetHook += ChangeText;
        }

        private string ChangeText(string key, string sheetTitle, string orig)
        {
            string text = JsonReader.getText(key);

            if (text == null) { return orig; }
            return text;
        }

        private void AddSpells()
        {
            ShadeSummon.CreateSpell();
            SoulDaggers.CreateSpell();
            ExplodeDive.CreateSpell();
            VoidTendrils.CreateSpell();
            XeroBlades.CreateSpell();
        }

        private void AddNailArts()
        {
            FlashDraw.CreateSpell();
        }
    }
}