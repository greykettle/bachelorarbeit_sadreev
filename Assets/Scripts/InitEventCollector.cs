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
        // Подписываемся на события кнопок
        languageButton.onClick.AddListener(OnLanguageButtonClicked);
        difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    void OnLanguageButtonClicked()
    {
        // Здесь можно реализовать логику выбора языка
        selectedLanguage = "Selected Language"; // замените это на логику выбора языка
        Debug.Log("Language selected: " + selectedLanguage);
    }

    void OnDifficultyButtonClicked()
    {
        // Здесь можно реализовать логику выбора сложности
        selectedDifficulty = "Selected Difficulty"; // замените это на логику выбора сложности
        Debug.Log("Difficulty selected: " + selectedDifficulty);
    }

    void OnStartGameButtonClicked()
    {
        // Здесь можно сохранить настройки и начать игру
        PlayerPrefs.SetString("Language", selectedLanguage);
        PlayerPrefs.SetString("Difficulty", selectedDifficulty);
        PlayerPrefs.Save();

        // Загрузить основную сцену игры
        SceneManager.LoadScene("MainGameScene"); // замените "MainGameScene" на имя вашей основной сцены
    }

    private void OnDestroy()
    {
        // Отписываемся от событий кнопок, чтобы избежать утечек памяти
        languageButton.onClick.RemoveListener(OnLanguageButtonClicked);
        difficultyButton.onClick.RemoveListener(OnDifficultyButtonClicked);
        startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
    }
}