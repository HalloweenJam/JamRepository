using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCLocalizated : LocalizedText
{
    private readonly string _nameKey = "Name";
    private Dictionary<string, List<string>> _dialogueData;
    private Dictionary<string, string> _nameData;

    public string Name
    {
        get
        {
            _nameData.TryGetValue(_nameKey, out var name);
            return name;
        }
    }

    public override void Awake()
    {
        UpdateLanguage();
        Settings.UpdateLanguageAction += UpdateLanguage;
    }

    public override void UpdateLanguage()
    {
        _dialogueData = LocalizationReader.GetLocalizationDialogueData(LocalizationData.LocalizationPath);
        _nameData = LocalizationReader.GetLocalizationName(LocalizationData.LocalizationPath); 
    }
  
    public string[] GetDialogueSentences()
    {
        try
        {
            string key = GetRandomKeyId().ToString();
            _dialogueData.TryGetValue("1", out List<string> sentences); // use random key, when added more dialogue in sheets
            return sentences.ToArray();
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
    }

    private int GetRandomKeyId()
    {
        System.Random random = new();
        int result = random.Next(2, _dialogueData.Count);
        return result;
    }
}


