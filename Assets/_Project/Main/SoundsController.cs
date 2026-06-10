using System;
using UnityEngine;

namespace _Project.Main
{
    public class SoundsController
    {
        public event Action OnMusicVolumeChanged;
        public event Action OnEffectsVolumeChanged;
        public event Action OnFootstepVolumeChanged;
        
        public float MusicVolume { get; private set; }
        public float EffectsVolume { get; private set; }
        public float FootstepsVolume { get; private set; }

        public void Setup()
        {
            MusicVolume = 0.1f;
            EffectsVolume = 0.3f;
            FootstepsVolume = 0.3f;
        }
        
        public void Setup(SoundsSaveData saveData)
        {
            MusicVolume = saveData.musicVolume;
            EffectsVolume = saveData.effectsVolume;
            FootstepsVolume = saveData.footstepsVolume;
        }

        public void ChangeMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp(volume, 0f, 1f);
            OnMusicVolumeChanged?.Invoke();
        }

        public void ChangeEffectsVolume(float volume)
        {
            EffectsVolume = Mathf.Clamp(volume, 0f, 1f);
            OnEffectsVolumeChanged?.Invoke();
        }

        public void ChangeFootstepsVolume(float volume)
        {
            FootstepsVolume = Mathf.Clamp(volume, 0f, 1f);
            OnFootstepVolumeChanged?.Invoke();
        }
    }
}