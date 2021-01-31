using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndGameTrigger : EventAction
{
    public Game Game;
    public Image Cover;

    public AudioSource MusicSource;
    public AudioSource RhythmSource;
    
    public AnimationCurve FadeOutCurve;
    
    public override void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false)
    {
        Game.EndGameTriggered = true;
        Game.StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float srcVol = MusicSource.volume;
        float targetVol = 0f;
        float srcAlpha = 0f;
        float targetAlpha = 1f;
        
        const float duration = 5f;
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
    }
}