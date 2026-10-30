using System;
using TMPro;
using UnityEngine;
using Zenject;

public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string EuText;
    [SerializeField] private string RuText;

    private TextMeshProUGUI _text;

    private LocalizationManager _localizationManager;
    
    
    [Inject]
    public void Construct(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;

        _localizationManager.OnLanguageChange += ChangeLanguage;

        _text = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeLanguage(bool isEnglish)
    {
        if (isEnglish)
        {
            _text.text = EuText;
        }
        else
        {
            _text.text = RuText;
        }
    }

    private void OnDisable()
    {
        if (_localizationManager != null)
        {
            _localizationManager.OnLanguageChange -= ChangeLanguage;
        }
    }

    private void OnEnable()
    {
        if (_localizationManager != null)
        {
            _localizationManager.OnLanguageChange += ChangeLanguage;
        }
    }
}
