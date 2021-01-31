using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct Subtitle
{
    public string Text;
    public float Start;
    public float Duration;
}

public class SplashScreen : MonoBehaviour
{
    public GameObject SkipText;

    public TextMeshProUGUI SubtitleText;
    public Subtitle[] Subtitles;

    public Image Image;
    public float FadeOutDuration;
    public float FadeOutInterval;

    public AudioSource Audio;

    private float _currTime;

    IEnumerator Start()
    {
        _currTime = 0;
        foreach (Subtitle subtitle in Subtitles)
        {
            StartCoroutine(SubtitleCoroutine(subtitle));
        }
        
        yield return new WaitForSeconds(5f);
        SkipText.SetActive(true);
    }

    IEnumerator SubtitleCoroutine(Subtitle subtitle)
    {
        yield return new WaitForSeconds(subtitle.Start);
        SubtitleText.text = subtitle.Text;
        yield return new WaitForSeconds(subtitle.Duration);
        SubtitleText.text = "";
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOutScene());
        }
    }

    IEnumerator FadeOutScene()
    {
        var interval = _currTime / FadeOutDuration;
        yield return new WaitForSeconds(FadeOutInterval);
        _currTime += FadeOutInterval;
        Image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, _currTime));
        Audio.volume = 1 - Mathf.Lerp(0, 1, _currTime);

        if (_currTime >= FadeOutDuration)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            StartCoroutine(FadeOutScene());
            
        }
    }
}