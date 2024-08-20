using UnityEngine;
using UnityEngine.UI;

public class LanguageToggleController : MonoBehaviour
{
    public Toggle languageToggle;
    public Image toggleBackground;
    public Sprite germanFlag;
    public Sprite britishFlag;

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
        toggleBackground.sprite = isOn ? germanFlag : britishFlag;
    }
}
