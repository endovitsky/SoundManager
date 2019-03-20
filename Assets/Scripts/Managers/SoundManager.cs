﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Managers
{
    /*
    Выбранный подход к решению является наиболее оптимизированным т.к. Ориентировочное время на выполнение: 1ч
    */
    public class SoundManager : MonoBehaviour, IInitializable, IUninitializable
    {
        [SerializeField]
        private List<AudioClip> AudioClips = new List<AudioClip>();

        private List<AudioSource> _currentlyPlaingAudioSources = new List<AudioSource>();

        private static System.Random _random = new System.Random();

        public bool IsPlayingSomething
        {
            get
            {
                return _currentlyPlaingAudioSources.Count > 0;
            }
        }

        private AudioSource CreateAudioSourceWithAudioClip(string name, float volume, bool loop)
        {
            var audioClip = AudioClips.FirstOrDefault(x => x.name == name);
            if (audioClip == null)
            {
                Debug.LogError("No audioClip was found with specified name :" + name);
                return null;
            }

            var gameObject = new GameObject();
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.loop = loop;

            return audioSource;
        }

        private AudioSource PlaySound(string name, float volume, bool loop)
        {
            var audioSource = CreateAudioSourceWithAudioClip(name, volume,  loop);

            audioSource.Play();
            _currentlyPlaingAudioSources.Add(audioSource);

            return audioSource;
        }

        public void Initialize()
        {

        }

        public void Uninitialize()
        {

        }

        public string GetRandomSoundName()
        {
            var randomSoundIndex = _random.Next(0, AudioClips.Count);

            return AudioClips[randomSoundIndex].name;
        }

        public string GetRandomcurrentlyPlaingAudioSourceName()
        {
            var randomSoundIndex = _random.Next(0, _currentlyPlaingAudioSources.Count);

            return _currentlyPlaingAudioSources[randomSoundIndex].name;
        }

        public void StopSound(string name)
        {
            var audioSource = _currentlyPlaingAudioSources.FirstOrDefault(x => x.name == name);
            if (audioSource == null)
            {
                Debug.LogError("No audioSource was found with specified name :" + name);
                return;
            }

            audioSource.Stop();
            _currentlyPlaingAudioSources.Remove(audioSource);

            Destroy(audioSource.gameObject);
        }

        public AudioSource PlaySound2D(string name, float volume, bool loop)
        {
            return PlaySound(name, volume, loop);
        }

        public AudioSource PlaySound3D(string name, float volume, bool loop, GameObject bindGameObject, bool fallowGameObject)
        {
            var audioSource = PlaySound(name, volume, loop);

            audioSource.gameObject.transform.position = bindGameObject.transform.position;
            if (fallowGameObject)
            {
                audioSource.gameObject.transform.SetParent(bindGameObject.transform);
            }

            return audioSource;
        }
    }
}
