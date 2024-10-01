using Newtonsoft.Json;
using System.Reflection;

namespace ExtraSpells;
public static class JsonReader
{
    // Path to your JSON file
    private static string jsonFilePath = "ExtraSpells.Resources.LanguageKeys.json";

    // Dictionary to hold the data
    private static Dictionary<string, string> spellData;

    static JsonReader()
    {
        TextAsset jsonFile = LoadJson(jsonFilePath);
        spellData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
    }

    public static string getText(string key)
    {
        if (spellData != null)
        {
            foreach (var entry in spellData)
            {
                if (entry.Key == key)
                {
                    return entry.Value;
                }
            }
        }
        return null;
    }

    private static TextAsset LoadJson(string path)
    {
        var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path);
        if (stream == null)
        {
            return null;
        }

        var String = new StreamReader(stream).ReadToEnd();
        TextAsset asset = new TextAsset(String);
        stream.Dispose();
        return asset;
    }
}