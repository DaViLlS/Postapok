using _Project.Localization;
using UnityEngine;

namespace _Project.Tutorial.Scripts
{
    [CreateAssetMenu(fileName = "Step data", menuName = "Tutorial/Step Data")]
    public class StepData : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private TextData[] titles;
        [SerializeField] private string description;
        [SerializeField] private TextData[] descriptions;

        public string GetTitle(LanguageType language)
        {
            for (var i = 0; i < titles.Length; i++)
            {
                if (titles[i].language == language)
                {
                    return titles[i].text;
                }
            }
            
            return string.Empty;
        }
        
        public string GetDescription(LanguageType language)
        {
            for (var i = 0; i < descriptions.Length; i++)
            {
                if (descriptions[i].language == language)
                {
                    return descriptions[i].text;
                }
            }
            
            return string.Empty;
        }
    }
}