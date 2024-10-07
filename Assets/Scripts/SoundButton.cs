using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private bool soundOn = true;
    [SerializeField] private Sprite turnOn;
    [SerializeField] private Sprite turnOff;
    [SerializeField] private Image image;
    [SerializeField] private AudioMixer main;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Volume", 1) == 1)
        {
            soundOn = true;
            image.sprite = turnOn;
            main.SetFloat("Volume", 0f);
        }
        else 
        {
            soundOn = false;
            image.sprite = turnOff;
            main.SetFloat("Volume", -80f);
        }
    }


    public void OnButtonClick()
    {
        if (soundOn == true)
        {
            soundOn = false;
            image.sprite = turnOff;
            main.SetFloat("Volume", -80f);
        }
        else 
        {
            soundOn = true;
            image.sprite = turnOn;
            main.SetFloat("Volume", 0f);
        }

        PlayerPrefs.SetInt("Volume", soundOn == true ? 1 : 0); //0 - mute, 1 - unmute
    }

}
