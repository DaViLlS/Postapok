using System.Collections.Generic;
using UnityEngine;

namespace _Project.Tutorial.ShortTutorial.Scripts
{
    [CreateAssetMenu(fileName = "Short GameTutorial Config", menuName = "Tutorial/ShortGameTutorialConfig")]
    public class ShortGameTutorialConfig : ScriptableObject
    {
        [SerializeField] private List<ShortGameTutorialData> shortGameTutorialInfos;
        
        public List<ShortGameTutorialData> ShortGameTutorialInfos => shortGameTutorialInfos;
    }
}