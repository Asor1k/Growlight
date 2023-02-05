using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam
{

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private List<AudioShot> audioShots;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource effectSource;


        private float lastEffect;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void CallFadeOut(float fadeTime)
        {
            StartCoroutine(FadeOut(fadeTime));
        }

        public void CallFadeIn(float fadeTime)
        {
            StartCoroutine(FadeIn(fadeTime));
        }


        public IEnumerator FadeOut(float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        public IEnumerator FadeIn(float FadeTime)
        {
            float startVolume = 0.2f;

            audioSource.volume = 0;
            audioSource.Play();

            while (audioSource.volume < 1.0f)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.volume = 1f;
        }

        public void PlayEffect(Audio shot)
        {
            if (Time.time - lastEffect < 0.1f) return;

            lastEffect = Time.time;
            AudioClip clip = audioShots.FirstOrDefault(audio => audio.audio == shot).clip;
            if (clip)
            {
                effectSource.PlayOneShot(clip);
            }
        }

        public void PlayMusic(Audio shot)
        {
            AudioClip clip = audioShots.FirstOrDefault(audio => audio.audio == shot).clip;

            if (clip)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

    }

    [Serializable]
    public struct AudioShot
    {
        public Audio audio;
        public AudioClip clip;
    }

    public enum Audio
    {
        PulseWave,
        Intro,
        Menu,
        GameAmbient,
        BuddieTouch,
        BranchBreak
    }

}