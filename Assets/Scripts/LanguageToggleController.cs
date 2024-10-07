using UnityEngine;
using UnityEngine.UI;

public class LanguageToggleController : MonoBehaviour
{
    public Toggle languageToggle;
    public Image toggleBackground;
    public Sprite germanFlag;
    public Sprite britishFlag;
    public Localizator localizator;

    void Start()
    {
        languageToggle.onValueChanged.AddListener(OnLanguageToggleChanged);
        UpdateToggleImage(languageToggle.isOn);
    }

    public void OnLanguageToggleChanged(bool isOn)
    {
        UpdateToggleImage(isOn);
    }

    void UpdateToggleImage(bool isOn)
    {
        if (isOn)
        {
            toggleBackground.sprite = germanFlag;
            localizator.ChangeLanguage("ger");
        }
        else
        {
            toggleBackground.sprite = britishFlag;
            localizator.ChangeLanguage("eng");

        }
    }
}
