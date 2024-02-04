using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] protected LocalizationData LocalizationData;
    [SerializeField] private string _key;
    private TextMeshProUGUI _text;

    public virtual void Awake() 
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateLanguage();
        Settings.UpdateLanguageAction += UpdateLanguage;
    }

    public virtual void UpdateLanguage() => _text.text = LocalizationData.GetTextData(_key);

    private void OnDestroy() => Settings.UpdateLanguageAction -= UpdateLanguage;

}
