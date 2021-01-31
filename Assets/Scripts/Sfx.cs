    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Sfx : MonoBehaviour
    {
        public AudioSource MusicAudioSource;
        public AudioSource RhythmAudioSource;
        public AudioClip MusicClip;
        public AudioClip RhythmClip;
        public AnimationCurve RhythmCurve;

        [Space]
        public AudioSource WalkAudioSource;
        public AudioClip Walk1Clip;
        public AudioClip Walk2Clip;

        private readonly List<AudioClip> _walkClips = new List<AudioClip>();

        void Start()
        {
            _walkClips.Add(Walk1Clip);
            _walkClips.Add(Walk2Clip);
        }

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

        public void PlayFootstep()
        {
            int r = Random.Range(0, _walkClips.Count);
            WalkAudioSource.PlayOneShot(_walkClips[r]);
        }
    }