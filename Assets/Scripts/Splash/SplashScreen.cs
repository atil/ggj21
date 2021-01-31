using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    IEnumerator Start()
    {
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
            SceneManager.LoadScene("SampleScene");
        }
    }
}