using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startManualButton;
    public Button startTutorialButton;
    public Button exitButton;
    public Toggle languageToggle;
    public LanguageToggleController languageToggleController;

    public GameObject mainMenuPanel;
    public GameObject manualSelectionPanel;
    public Button oneStageButton;
    public Button twoStageButton;

    void Start()
    {
        //startManualButton.onClick.AddListener(OnStartManualClicked);
       // startTutorialButton.onClick.AddListener(OnStartTutorialClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        languageToggle.onValueChanged.AddListener(OnLanguageToggleChanged);


        manualSelectionPanel.SetActive(false);

    }

    void OnStartManualClicked()
    {
        mainMenuPanel.SetActive(false);
        manualSelectionPanel.SetActive(true);
        languageToggle.gameObject.SetActive(false);
    }

    void OnStartTutorialClicked()
    {
        Debug.Log("Start Tutorial");
        languageToggle.gameObject.SetActive(false);
    }

    void OnExitClicked()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    void OnLanguageToggleChanged(bool isOn)
    {
        languageToggleController.OnLanguageToggleChanged(isOn);
        Debug.Log("Change Language: " + (isOn ? "German" : "English"));
    }

}
