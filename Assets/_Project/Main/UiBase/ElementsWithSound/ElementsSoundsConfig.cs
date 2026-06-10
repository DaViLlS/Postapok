using UnityEngine;

namespace _Project.Main.UiBase.ElementsWithSound
{
    [CreateAssetMenu(menuName = "Sounds/Elements Sounds Config", fileName = "ElementsSoundsConfig")]
    public class ElementsSoundsConfig : ScriptableObject
    {
        [SerializeField] private AudioClip buttonAudioClip;
        
        public AudioClip ButtonAudioClip => buttonAudioClip;
    }
}