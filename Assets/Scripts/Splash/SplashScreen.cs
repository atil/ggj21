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

    public Image Cover;
    public AnimationCurve CoverCurve;

    public AudioSource Audio;

    private bool _isStartingGame = false;

    IEnumerator Start()
    {
        foreach (Subtitle subtitle in Subtitles)
        {
            StartCoroutine(SubtitleCoroutine(subtitle));
        }
        
        // Fade in
        const float coverDuration = 1f;
        for (float f = 0; f < coverDuration; f += Time.deltaTime)
        {
            Color c = Cover.color;
            c.a = Mathf.Lerp(1f, 0f, CoverCurve.Evaluate(f / coverDuration));
            Cover.color = c;
            yield return null;
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
        if (!_isStartingGame && Input.GetKeyDown(KeyCode.Space))
        {
            _isStartingGame = true;
            StartCoroutine(FadeOutScene());
        }
    }

    IEnumerator FadeOutScene()
    {
        float srcVol = Audio.volume;
        const float coverDuration = 2f;
        for (float f = 0; f < coverDuration; f += Time.deltaTime)
        {
            float t = CoverCurve.Evaluate(f / coverDuration);
            
            // Image
            Color c = Cover.color;
            c.a = Mathf.Lerp(0f, 1f, t);
            Cover.color = c;
            
            // Audio
            Audio.volume = Mathf.Lerp(srcVol, 0f, t);
            
            yield return null;
        }
        
        SceneManager.LoadScene("SampleScene");
    }
}