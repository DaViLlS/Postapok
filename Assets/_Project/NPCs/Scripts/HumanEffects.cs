using System.Collections;
using _Project.Main;
using UnityEngine;
using Zenject;

namespace _Project.NPCs.Scripts
{
    public class HumanEffects : MonoBehaviour
    {
        [Inject] private SoundsController _soundsController;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private AudioSource effectsAudioSource;
        [SerializeField] private AudioSource voiceAudioSource;
        [SerializeField] private AudioSource specFootstepAudioSource;
        [SerializeField] private AudioSource archerAudioSource;
        [Header("Audio clips")] 
        [SerializeField] private AudioClip[] applyDamageClips;
        [SerializeField] private AudioClip[] deathClips;
        [SerializeField] private AudioClip[] bowStringClips;
        [SerializeField] private AudioClip[] specFootstepClips;
        [SerializeField] private AudioClip[] throwProjectileClips;
        [Header("Volume")]
        [SerializeField] private float applyDamagePitch;
        [Space]
        [SerializeField] private float deathPitch;
        [Space]
        [SerializeField] private float specFootstepPitch;
        [Space]
        [SerializeField] private float bowStringPitch;
        [SerializeField] private float throwPitch;
        
        public void ApplyVisualDamage()
        {
            if (applyDamageClips.Length > 0)
            {
                var clip = applyDamageClips[Random.Range(0, applyDamageClips.Length)];
                effectsAudioSource.volume = _soundsController.EffectsVolume;
                effectsAudioSource.pitch = applyDamagePitch;
                effectsAudioSource.PlayOneShot(clip);
            }
            
            StartCoroutine(VisualDamageCoroutine());
        }

        private IEnumerator VisualDamageCoroutine()
        {
            spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(0.2f);
            
            spriteRenderer.color = Color.white;
        }

        public void ApplyVisualDeath()
        {
            if (deathClips.Length > 0)
            {
                var clip = deathClips[Random.Range(0, deathClips.Length)];
                effectsAudioSource.volume = _soundsController.EffectsVolume;
                effectsAudioSource.pitch = deathPitch;
                effectsAudioSource.PlayOneShot(clip);
            }
        }

        public void ApplySpecFootstep()
        {
            if (specFootstepClips.Length > 0)
            {
                var clip = specFootstepClips[Random.Range(0, specFootstepClips.Length)];
                specFootstepAudioSource.volume = _soundsController.EffectsVolume;
                specFootstepAudioSource.pitch = specFootstepPitch;
                specFootstepAudioSource.PlayOneShot(clip);
            }
        }
        
        public void ApplyRangeString()
        {
            if (bowStringClips.Length > 0)
            {
                var clip = bowStringClips[Random.Range(0, bowStringClips.Length)];
                archerAudioSource.volume = _soundsController.EffectsVolume;
                archerAudioSource.pitch = bowStringPitch;
                archerAudioSource.PlayOneShot(clip);
            }
        }

        public void ApplyProjectileRelease()
        {
            if (throwProjectileClips.Length > 0)
            {
                var clip = throwProjectileClips[Random.Range(0, throwProjectileClips.Length)];
                effectsAudioSource.volume = _soundsController.EffectsVolume;
                effectsAudioSource.pitch = throwPitch;
                effectsAudioSource.PlayOneShot(clip);
            }
        }
    }
}