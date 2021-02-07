using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour
{
    public Game Game;
    public Image Cover;

    public AudioSource MusicSource;
    public AudioSource RhythmSource;
    
    public AnimationCurve FadeOutCurve;
    
    private BoxCollider2D _collider;
    
    private bool _isPlayed = false;
    
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _isPlayed = false;
    }
    
    private void Update()
    {
        if (_isPlayed)
        {
            return;
        }
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size , 0);
        foreach (Collider2D c in colliders)
        {
            if (c == _collider)
            {
                continue;
            }

            if (c.name == "Player")
            {
                _isPlayed = true;
                StartCoroutine(FadeOutCoroutine());
            }
        }
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        Game.EndGameTriggered = true;
        yield return new WaitForSeconds(2f);
        float srcVol = MusicSource.volume;
        float targetVol = 0f;
        float srcAlpha = 0f;
        float targetAlpha = 1f;
        
        const float duration = 6f;
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = FadeOutCurve.Evaluate(f / duration);

            MusicSource.volume = Mathf.Lerp(srcVol, targetVol, t);
            RhythmSource.volume = Mathf.Lerp(srcVol, targetVol, t);
            var c = Cover.color;
            c.a = Mathf.Lerp(srcAlpha, targetAlpha, t);
            Cover.color = c;
            
            yield return null;
        }

        SceneManager.LoadScene("End");
    }
}