using System;
using _Project.Localization;
using UnityEngine;

namespace _Project.TasksAndDialogues.Dialogues.Scripts
{
    [Serializable]
    public class DialogueData
    {
        public string speakerName;
        public TextData[] speakerNames;
        public Sprite speakerIcon;
        public string dialogueText;
        public TextData[] dialogueTexts;
        public float delay;
        public bool isLeft;
        
        public string GetSpeakerName(LanguageType language)
        {
            for (var i = 0; i < speakerNames.Length; i++)
            {
                if (speakerNames[i].language == language)
                {
                    return speakerNames[i].text;
                }
            }
            
            return string.Empty;
        }
        
        public string GetDialogueText(LanguageType language)
        {
            for (var i = 0; i < dialogueTexts.Length; i++)
            {
                if (dialogueTexts[i].language == language)
                {
                    return dialogueTexts[i].text;
                }
            }
            
            return string.Empty;
        }
    }
}