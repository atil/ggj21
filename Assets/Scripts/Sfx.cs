    using System.Collections;
    using UnityEngine;

    public class Sfx : MonoBehaviour
    {
        public AudioSource MusicAudioSource;
        public AudioSource RhythmAudioSource;
        public AudioClip MusicClip;
        public AudioClip RhythmClip;
        public AnimationCurve RhythmCurve;

        public void TraverseEffect(float duration)
        {
            StartCoroutine(TransitionEffectCoroutine(duration));
        }

        private IEnumerator TransitionEffectCoroutine(float duration)
        {
            float highVolume = RhythmAudioSource.volume;
            float lowVolume = RhythmAudioSource.volume * 0.1f;
            for (float f = 0f; f < duration; f += Time.deltaTime)
            {
                RhythmAudioSource.volume = RhythmCurve.Evaluate(f / duration);
                yield return null;
            }

            RhythmAudioSource.volume = highVolume;
        }
    }