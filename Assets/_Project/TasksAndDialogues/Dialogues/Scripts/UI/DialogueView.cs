using System;
using _Project.Localization;
using _Project.Main.UiBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.TasksAndDialogues.Dialogues.Scripts.UI
{
    public class DialogueView : GameScreen
    {
        public event Action OnNextStepClicked;

        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private Image leftSpeakerImage;
        [SerializeField] private Image rightSpeakerImage;
        [SerializeField] private TMP_Text leftSpeakerName;
        [SerializeField] private TMP_Text rightSpeakerName;
        [SerializeField] private TMP_Text dialogueText;
        
        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public void ChangeStep(DialogueData dialogueData)
        {
            if (dialogueData.isLeft)
            {
                rightSpeakerImage.gameObject.SetActive(false);
                leftSpeakerImage.gameObject.SetActive(true);
                leftSpeakerImage.sprite = dialogueData.speakerIcon;
                
                rightSpeakerName.gameObject.SetActive(false);
                leftSpeakerName.gameObject.SetActive(true);
                leftSpeakerName.text = dialogueData.GetSpeakerName(_localizationController.ChosenLanguage);
            }
            else
            {
                leftSpeakerImage.gameObject.SetActive(false);
                rightSpeakerImage.gameObject.SetActive(true);
                rightSpeakerImage.sprite = dialogueData.speakerIcon;
                
                leftSpeakerName.gameObject.SetActive(false);
                rightSpeakerName.gameObject.SetActive(true);
                rightSpeakerName.text = dialogueData.GetSpeakerName(_localizationController.ChosenLanguage);
            }
        }

        public void PrintText(string text)
        {
            dialogueText.text = text;
        }

        public void ClickNextStep()
        {
            Debug.Log("КЛИК ЕПТА");
            OnNextStepClicked?.Invoke();
        }
    }
}