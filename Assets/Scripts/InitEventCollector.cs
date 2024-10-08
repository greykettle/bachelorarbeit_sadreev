using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitEventCollector : MonoBehaviour
{
    public Button languageButton;
    public Button difficultyButton;
    public Button startGameButton;

    private string selectedLanguage = "English";
    private string selectedDifficulty = "Normal";

    void Start()
    {
        
        languageButton.onClick.AddListener(OnLanguageButtonClicked);
        difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    void OnLanguageButtonClicked()
    {
       
        selectedLanguage = "Selected Language"; 
        Debug.Log("Language selected: " + selectedLanguage);
    }

    void OnDifficultyButtonClicked()
    {
        
        selectedDifficulty = "Selected Difficulty"; 
        Debug.Log("Difficulty selected: " + selectedDifficulty);
    }

    void OnStartGameButtonClicked()
    {
        
        PlayerPrefs.SetString("Language", selectedLanguage);
        PlayerPrefs.SetString("Difficulty", selectedDifficulty);
        PlayerPrefs.Save();

    
        SceneManager.LoadScene("MainGameScene"); 
    }

    private void OnDestroy()
    {
       
        languageButton.onClick.RemoveListener(OnLanguageButtonClicked);
        difficultyButton.onClick.RemoveListener(OnDifficultyButtonClicked);
        startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
    }
}