using UnityEngine;
using Zenject;

namespace _Project.Main
{
    public class MusicLoopController : MonoBehaviour
    {
        [Inject] protected SoundsController SoundsController;
        
        [SerializeField] protected AudioSource source;

        protected virtual void Start()
        {
            SoundsController.OnMusicVolumeChanged += ChangeMusicVolume;
            source.volume = SoundsController.MusicVolume;
        }

        private void OnDestroy()
        {
            SoundsController.OnMusicVolumeChanged -= ChangeMusicVolume;
        }

        private void ChangeMusicVolume()
        {
            source.volume = SoundsController.MusicVolume;
        }
    }
}