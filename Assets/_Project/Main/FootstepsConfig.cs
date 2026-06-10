using UnityEngine;

namespace _Project.Main
{
    [CreateAssetMenu(fileName = "FootstepsConfig", menuName = "Footsteps Config")]
    public class FootstepsConfig : ScriptableObject
    {
        [SerializeField] private FootstepsSystem.TileSoundPair[] tileSoundPairs;
        
        public FootstepsSystem.TileSoundPair[] TileSoundPairs => tileSoundPairs;
    }
}