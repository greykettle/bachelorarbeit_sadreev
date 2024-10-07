using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    public List<AudioHandler> speechs;
    public AudioSource speechSource;
    public bool isOn;
    public float firstSpeechDelay;
    public float SpeechDelay;
    public Localizator localizator;
    public void PlaySpeech(int stepIndex)
    {
        if (isOn)
        {
            if (stepIndex == 0)
            {
                StartCoroutine(PlaySpeechWithDelay(firstSpeechDelay, stepIndex));
                Invoke("PlaySpeechWithDelay", firstSpeechDelay);
            }
            else
            {
                StartCoroutine(PlaySpeechWithDelay(SpeechDelay, stepIndex));

            }
        }
    }

    private IEnumerator PlaySpeechWithDelay(float delay, int index)
    {
        if (speechSource.isPlaying)
        { speechSource.Stop(); }
        yield return new WaitForSeconds(delay);
        speechSource.PlayOneShot(localizator.currentLanguage == "eng" ? speechs[index].eng : speechs[index].ger);
    }
}

[Serializable]
public class AudioHandler
{
     public AudioClip ger, eng;
}