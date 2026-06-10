using System;
using System.Collections;
using _Project.Localization;
using _Project.TasksAndDialogues.Dialogues.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Project.TasksAndDialogues.Dialogues.Scripts
{
    public class DialoguesController : MonoBehaviour
    {
        public event Action OnDialogueCompleted;
        
        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private DialogueView dialogueView;
        
        private DialogueConfig _currentDialogue;
        private Coroutine _printTextCoroutine;
        private bool _isPrintingText;
        private int _dialogueStep;

        private void Awake()
        {
            dialogueView.OnNextStepClicked += PerformDialogueClicked;
        }

        private void OnDestroy()
        {
            dialogueView.OnNextStepClicked -= PerformDialogueClicked;
        }

        public void StartDialogue(DialogueConfig dialogueConfig)
        {
            if (_printTextCoroutine != null)
                StopCoroutine(_printTextCoroutine);
            
            _currentDialogue = dialogueConfig;
            _dialogueStep = 0;

            dialogueView.Open();
            var dialogue = _currentDialogue.GetDialogue(_dialogueStep);
            dialogueView.ChangeStep(dialogue);
            _printTextCoroutine = StartCoroutine(PrintTextWithDelay(dialogue.GetDialogueText(_localizationController.ChosenLanguage), dialogue.delay));
        }

        private void PerformDialogueClicked()
        {
            if (_isPrintingText)
            {
                _isPrintingText = false;
                StopCoroutine(_printTextCoroutine);
                dialogueView.PrintText(_currentDialogue.GetDialogue(_dialogueStep).GetDialogueText(_localizationController.ChosenLanguage));
            }
            else
            {
                ChangeDialogueToNextStep();
            }
        }

        public void ChangeDialogueToNextStep()
        {
            Debug.Log("Dialogue index: " + _dialogueStep);
            _dialogueStep++;

            if (_currentDialogue.IsEnd(_dialogueStep))
            {
                Debug.Log("Dialogue " + _currentDialogue.name + " ended");
                CompleteDialogue();
                return;
            }

            var dialogue = _currentDialogue.GetDialogue(_dialogueStep);
            dialogueView.ChangeStep(dialogue);

            _isPrintingText = true;
            
            if (_printTextCoroutine != null)
                StopCoroutine(_printTextCoroutine);
            
            _printTextCoroutine = StartCoroutine(PrintTextWithDelay(dialogue.GetDialogueText(_localizationController.ChosenLanguage), dialogue.delay));
        }

        private IEnumerator PrintTextWithDelay(string text, float delay)
        {
            _isPrintingText = true;
            var newText = string.Empty;
            
            foreach (char c in text)
            {
                newText = newText + c;
                dialogueView.PrintText(newText);
                
                yield return new WaitForSeconds(delay);
            }

            _isPrintingText = false;
        }

        private void CompleteDialogue()
        {
            if (_printTextCoroutine != null)
                StopCoroutine(_printTextCoroutine);
            
            dialogueView.Close();
            
            OnDialogueCompleted?.Invoke();
        }
    }
}