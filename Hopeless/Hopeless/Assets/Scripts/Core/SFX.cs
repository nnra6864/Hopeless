using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Core
{
    public class SFX : MonoBehaviour
    {
        [Serializable]
        public class Sound
        {
            public AudioClip Clip;
            public AudioMixerGroup Mixer;
            public bool Loop;
            public string Name;
            [Range(0f, 1f)]
            public float Volume = 1;
            [Range(-3f, 3f)]
            public float Pitch = 1;
            [HideInInspector] public AudioSource Source;

            public Sound() { }
            public Sound(Sound sound)
            {
                Clip = sound.Clip;
                Mixer = sound.Mixer;
                Loop = sound.Loop;
                Name = sound.Name;
                Volume = sound.Volume;
                Pitch = sound.Pitch;
                Source = sound.Source;
            }
        }
        public Sound[] Sounds;

        private static SFX _instance;
        public static SFX Instance
        {
            get => _instance;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            AddSources();
        }

        void AddSources()
        {
            foreach (var sound in Sounds)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = sound.Clip;
                source.outputAudioMixerGroup = sound.Mixer;
                source.loop = sound.Loop;
                source.volume = sound.Volume;
                source.pitch = sound.Pitch;
                sound.Source = source;
            }
        }

        public static void PlaySFX(GameObject target, string SoundName)
        {
            var source = target.AddComponent<AudioSource>();
            Sound sound = new(Array.Find(Instance.Sounds, x => x.Name == SoundName));
            sound.Source = source;
            source.clip = sound.Clip;
            source.loop = sound.Loop;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.outputAudioMixerGroup = sound.Mixer;
            source.Play();
            if (!sound.Loop) _instance.StartCoroutine(DestroyAudioSource(source));
        }

        static IEnumerator DestroyAudioSource(AudioSource source)
        {
            var length = source.clip.length;
            yield return new WaitForSeconds(length + 0.5f);
            if (source == null) yield break;
            Destroy(source);
        }

        public static void PlaySound(string SoundName)
        {
            Array.Find(Instance.Sounds, x => x.Name == SoundName).Source.Stop();
        }

        public static void StopSound(string SoundName)
        {
            Array.Find(Instance.Sounds, x => x.Name == SoundName).Source.Stop();
        }
    }
}