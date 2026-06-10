using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Main.UiBase.ElementsWithSound
{
    public class ButtonSound : MonoBehaviour
    {
        [Inject] private AudioSource _uiAudioSource;
        [Inject] private ElementsSoundsConfig _elementsSoundsConfig;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(MakeSound);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void MakeSound()
        {
            _uiAudioSource.PlayOneShot(_elementsSoundsConfig.ButtonAudioClip);
        }
    }
}