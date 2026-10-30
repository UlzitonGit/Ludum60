using System;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private bool _isEnglish;

    public Action<bool> OnLanguageChange;

    public void Init()
    {
        
        int i = PlayerPrefs.GetInt("isEnglish");
        
        if (i == 0) i = 1;
        
        PlayerPrefs.SetInt("isEnglish", i);
        
        print(i);
        _isEnglish = i == 1;
    }
    public void ChangeLanguage(int isEnglish)
    {
        int i = PlayerPrefs.GetInt("isEnglish") * isEnglish;
        _isEnglish = i == 1;
        
        PlayerPrefs.SetInt("isEnglish", i);
        
        OnLanguageChange?.Invoke(_isEnglish);
    }

    private void Start()
    {
        OnLanguageChange?.Invoke(_isEnglish);
    }
}
