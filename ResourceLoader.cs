

using HutongGames.PlayMaker.Actions;
using System.Reflection;
using UnityEngine;

namespace ExtraSpells
{
    internal static class ResourceLoader
    {
        public static GameObject shadeenemy;
        public static GameObject soulDagger;
        public static GameObject explosionObject;
        public static GameObject tendrilObject;
        public static GameObject xerobladeObject;

        public static void Initialise(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            shadeenemy = preloadedObjects["Abyss_15"]["Shade Sibling (32)"];
            UnityEngine.Object.DontDestroyOnLoad(shadeenemy);

            GameObject hkprime = preloadedObjects["GG_Hollow_Knight"]["Battle Scene/HK Prime"];
            UnityEngine.Object.DontDestroyOnLoad(hkprime);
            PlayMakerFSM hkfsm = hkprime.LocateMyFSM("Control");
            soulDagger = hkfsm.GetAction<FlingObjectsFromGlobalPoolTime>("SmallShot LowHigh", 2).gameObject.Value;
            tendrilObject = hkprime.Child("Tendrils");

            GameObject zoteBalloon = preloadedObjects["GG_Grey_Prince_Zote"]["Zote Balloon"];
            UnityEngine.Object.DontDestroyOnLoad(zoteBalloon);
            PlayMakerFSM balloonfsm = zoteBalloon.LocateMyFSM("Control");
            explosionObject = balloonfsm.GetAction<CreateObject>("Explode", 1).gameObject.Value;

            GameObject xero = preloadedObjects["GG_Ghost_Xero"]["Warrior/Ghost Warrior Xero"];
            UnityEngine.Object.DontDestroyOnLoad(xero);
            PlayMakerFSM xerofsm = xero.LocateMyFSM("Attacking");
            xerobladeObject = xero.Child("Sword 3");
        }
        public static Texture2D LoadTexture2D(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            MemoryStream memoryStream = new((int)stream.Length);
            stream.CopyTo(memoryStream);
            stream.Close();
            var bytes = memoryStream.ToArray();
            memoryStream.Close();

            var texture2D = new Texture2D(1, 1);
            _ = texture2D.LoadImage(bytes);
            texture2D.anisoLevel = 0;

            return texture2D;
        }

        public static Sprite LoadSprite(string path)
        {
            Texture2D texture = LoadTexture2D(path);
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one / 2, 100.0f);
        }
    }
}
