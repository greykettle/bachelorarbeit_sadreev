using Assets.Scripts.Localize;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localizator : MonoBehaviour
{ 
    public TextHandler[] textHandlers;
    public string currentLanguage;
    void Start()
    {
        currentLanguage = PlayerPrefs.GetString("language", "eng");
        TranslateAllTexts(currentLanguage);

    }
    public void ChangeLanguage(string lang)
    {
        currentLanguage = lang;
        PlayerPrefs.SetString("language", currentLanguage);
        TranslateAllTexts(currentLanguage);

    }

    private void TranslateAllTexts(string lang)
    {
        for (int i = 0; i < textHandlers.Length; i++)
        {
            textHandlers[i].ChangeLanguage(lang);
        }
    }
   

}
