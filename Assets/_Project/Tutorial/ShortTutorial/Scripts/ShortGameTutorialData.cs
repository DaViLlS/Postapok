using System;
using _Project.Localization;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Tutorial.ShortTutorial.Scripts
{
    [Serializable]
    public class ShortGameTutorialData
    {
        public Sprite tutorialSprite;
        public string tutorialText;
        public TextData[] tutorialTexts;

        public string GetText(LanguageType language)
        {
            for (var i = 0; i < tutorialTexts.Length; i++)
            {
                if (tutorialTexts[i].language == language)
                {
                    return tutorialTexts[i].text;
                }
            }
            
            return string.Empty;
        }
    }
}