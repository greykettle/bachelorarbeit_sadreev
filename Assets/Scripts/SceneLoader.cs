using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  public void  LoadSceneByIndex(int sceneNumber)
    {
        
        PlayerPrefs.SetInt("ChoosedSceneIndex", sceneNumber);
        SceneManager.LoadScene(6);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void LoadSceneBySavedIndex()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("ChoosedSceneIndex"));
    }
}
