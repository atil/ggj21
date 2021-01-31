using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image Image;
    public float FadeOutDuration;
    public float FadeOutInterval;

    public AudioSource Audio1;
    public AudioSource Audio2;

    private float _currTime;

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            StartCoroutine(FadeInScene());
        }
    }
    
    IEnumerator FadeInScene()
    {
        var interval = _currTime / FadeOutDuration;
        yield return new WaitForSeconds(FadeOutInterval);
        _currTime += FadeOutInterval;
        var c = Image.color;
        c.a = 1 - Mathf.Lerp(0, 1, _currTime);
        Image.color = c;
        Audio1.volume = Mathf.Lerp(0, 1, _currTime);
        Audio2.volume = Mathf.Lerp(0, 1, _currTime);

        if (_currTime >= FadeOutDuration)
        {
            c = Image.color;
            c.a = 0;
            Image.color = c;
            Audio1.volume = 1;
            Audio2.volume = 1;
        }
        else
        {
            StartCoroutine(FadeInScene());
            
        }
    }
}
