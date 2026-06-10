using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Main
{
    public class MainMusicLoopController : MusicLoopController
    {
        [SerializeField] private float fadeTime = 1f;
        [SerializeField] private AudioClip[] musicClips;

        private Queue<AudioClip> _trackQueue = new Queue<AudioClip>();
        private Coroutine _currentFadeCoroutine;

        protected override void Start()
        {
            base.Start();
            
            RegenerateTrackQueue();
            PlayNextTrack();
        }

        private void RegenerateTrackQueue()
        {
            var tempClips = musicClips.ToList();

            for (var i = 0; i < musicClips.Length; i++)
            {
                var index = Random.Range(0, tempClips.Count - 1);
                tempClips.RemoveAt(index);
                _trackQueue.Enqueue(musicClips[index]);
            }
        }
        
        void Update()
        {
            // Проверяем, закончился ли текущий трек
            if (source.isPlaying == false && _trackQueue.Count > 0)
            {
                PlayNextTrack();
            }
        }
        
        public void PlayNextTrack()
        {
            if (_trackQueue.Count > 0)
            {
                var nextTrack = _trackQueue.Dequeue();
                StartCoroutine(FadeToNewTrack(nextTrack));
            }
        }
        
        private IEnumerator FadeToNewTrack(AudioClip newTrack)
        {
            // Затухание текущего трека
            if (source.isPlaying)
            {
                float startVolume = source.volume;
                float elapsed = 0f;

                while (elapsed < fadeTime)
                {
                    elapsed += Time.deltaTime;
                    source.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
                    yield return null;
                }
            }

            // Переключение на новый трек
            source.Stop();
            source.clip = newTrack;
            source.volume = 0f;
            source.Play();

            // Нарастание громкости нового трека
            float elapsedFadeIn = 0f;
            
            while (elapsedFadeIn < fadeTime)
            {
                elapsedFadeIn += Time.deltaTime;
                source.volume = Mathf.Lerp(0f, SoundsController.MusicVolume, elapsedFadeIn / fadeTime);
                yield return null;
            }

            source.volume = SoundsController.MusicVolume;
        }
    }
}