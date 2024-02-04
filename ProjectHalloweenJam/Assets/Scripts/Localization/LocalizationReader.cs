using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationReader
{
    private static string s_pathLocalization => Path.Combine(Application.dataPath, "Scripts", "Localization");
    private static string s_pathDialogue => Path.Combine(Application.dataPath, "DialogueData");

    private static int _languageId = 0;

    public static Dictionary<string, string> GetLocalizationData(string fileName)
    {
        string path = Path.Combine(s_pathLocalization, fileName);

        Dictionary<string, string> localization = new(); 
        using (StreamReader reader = new StreamReader(path))
        {
            string row;
            while ((row = reader.ReadLine()) != null)
                localization.Add(row.Split(';')[0], row.Split(';')[_languageId + 1]);
        }
        return localization;
    }

    public static Dictionary<string, List<string>> GetLocalizationDialogueData(string fileName)
    {
        string path = Path.Combine(s_pathDialogue, fileName);

        Dictionary<string, List<string>> localization = new();
        using (StreamReader reader = new StreamReader(path))
        {
            string row;
            string currentKey = null;
            string currentValue = null;

            while ((row = reader.ReadLine()) != null)
            {
                currentKey = row.Split(';')[0];
                currentValue = row.Split(';')[_languageId + 1];

                if (!localization.ContainsKey(currentKey))
                    localization[currentKey] = new List<string>();
       
                localization[currentKey].Add(currentValue);
            }
        }
        return localization;
    }

    public static Dictionary<string, string> GetLocalizationName(string fileName)
    {
        string path = Path.Combine(s_pathDialogue, fileName);

        Dictionary<string, string> localization = new();
        using (StreamReader reader = new StreamReader(path))
        {
            string row;
            row = reader.ReadLine(); 
            if ((row = reader.ReadLine()) != null)           
                localization.Add(row.Split(';')[0], row.Split(';')[_languageId + 1]);         
        }
        return localization;
    }

    public static void SetLanguageId(int value) => _languageId = value;
}
