using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationData")]
public class LocalizationData : ScriptableObject
{
    [SerializeField] private string _localizationField;
    [SerializeField] private string _localizationPath;

    private Dictionary<string, string> _localizationData => LocalizationReader.GetLocalizationData(_localizationPath);
    public string LocalizationPath => _localizationPath;

    public string GetTextData(string key) 
    {
        try
        {
            _localizationData.TryGetValue(key, out string text);
            return text;
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
}

