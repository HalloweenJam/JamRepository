using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private List<ResItem> _resolutionItems;

    [SerializeField] private TextMeshProUGUI _resText;
    [SerializeField] private Toggle _fullScreenTog;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effetsSlider;
    [SerializeField] private TMP_Dropdown _language;

    [SerializeField] private AudioMixerGroup _music;
    [SerializeField] private AudioMixerGroup _effects;

    private ResItem _resolutionItem;
    private bool _isFirstStartGame = true;

    private int _currentResIndex;

    public static Action UpdateLanguageAction;

    private void Start()
    {
        LoadSettings();
        SetSettings();
    }

    private void SetSettings()
    {
        SetResolutionItem();

        Screen.fullScreen = _fullScreenTog.isOn;
        FullScreenMode fullScreen = _fullScreenTog.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(_resolutionItem.Width, _resolutionItem.Height, fullScreen);

        _music.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, _musicSlider.value));
        _effects.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-80, 0, _effetsSlider.value));

        LocalizationReader.SetLanguageId(_language.value);
        UpdateLanguageAction?.Invoke();
    }

    public void SaveSettings()
    {
        SetSettings();

        SettingsData data = new SettingsData();

        data.ResItem = _resolutionItem;
        data.FullScreen = _fullScreenTog.isOn;
        data.MusicVolume = _musicSlider.value;
        data.EffectsVolume = _effetsSlider.value;
        data.FirstStartGame = _isFirstStartGame;
        data.Language = _language.value;
        
        SettingsSaveLoadUtils.SaveSettingsData(data);
    }

    private void LoadSettings()
    {
        SettingsData data = SettingsSaveLoadUtils.LoadSettingsData();

        if (data == null)
        {
            SetDefaultValue();
            return;
        }

        _resolutionItem = data.ResItem;
        _fullScreenTog.isOn = data.FullScreen;
        _musicSlider.value = data.MusicVolume;
        _effetsSlider.value = data.EffectsVolume;
        _isFirstStartGame = data.FirstStartGame;
        _language.value = data.Language; 

        _currentResIndex = _resolutionItems.IndexOf(_resolutionItem);
    }

    public void OnResolutionButtonClick(bool isLeftButton)
    {
        if (isLeftButton && _currentResIndex > 0)
            _currentResIndex--;
        else if (!isLeftButton && _currentResIndex < _resolutionItems.Count - 1)
            _currentResIndex++;

        SetResolutionItem();
    }

    private void SetResolutionItem()
    {
        _resolutionItem = _resolutionItems[_currentResIndex];
        _resText.text = $"{_resolutionItem.Width}" + " x " + $"{_resolutionItem.Height}";
    }

    private void SetDefaultValue()
    {
        _resolutionItem = _resolutionItems[_currentResIndex];
        _currentResIndex = 4;
        _fullScreenTog.isOn = true;
        _musicSlider.value = 0.5f;
        _effetsSlider.value = 0.5f;
        _isFirstStartGame = true;
        _language.value = 0;

        SetSettings();
    }
}

[Serializable]
public struct ResItem
{
    public int Width;
    public int Height;
}