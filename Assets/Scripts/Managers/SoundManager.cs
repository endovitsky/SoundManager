﻿using System.Collections.Generic;
using System.Linq;
using Pooling;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SoundManager : MonoBehaviour, IInitializable, IUninitializable
    {
        [SerializeField]
        private List<AudioClip> AudioClips = new List<AudioClip>();

        [SerializeField]
        private AudioSource _audioSourcePrefab;

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

            var audioSource = Pool.Spawn(_audioSourcePrefab);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.loop = loop;

            Debug.Log(string.Format("Audio source with audio clip({0}) was created.", audioSource.clip.name));

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

            Pool.Despawn(audioSource.gameObject);

            Debug.Log(string.Format("Stopped to playing audio source with audio clip({0}).", audioSource.clip.name));
        }

        public AudioSource PlaySound2D(string name, float volume, bool loop)
        {
            var audioSource = PlaySound(name, volume, loop);

            Debug.Log(string.Format("Started to playing audio source with audio clip({0}) as 2D.", audioSource.clip.name));

            return audioSource;
        }

        public AudioSource PlaySound3D(string name, float volume, bool loop, GameObject bindGameObject, bool fallowGameObject)
        {
            var audioSource = PlaySound(name, volume, loop);

            Debug.Log(string.Format("Started to playing audio source with audio clip({0}) as 3D.", audioSource.clip.name));

            audioSource.gameObject.transform.position = bindGameObject.transform.position;
            if (fallowGameObject)
            {
                audioSource.gameObject.transform.SetParent(bindGameObject.transform);

                Debug.Log(string.Format("Setted audio source with audio clip({0}) to fallow game object({1}).",
                    audioSource.clip.name, bindGameObject.name));
            }

            return audioSource;
        }
    }
}
