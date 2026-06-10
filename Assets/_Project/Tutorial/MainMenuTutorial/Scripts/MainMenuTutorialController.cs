using System;
using System.Collections.Generic;
using _Project.CameraControlling;
using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Tutorial.MainMenuTutorial.Scripts
{
    public class MainMenuTutorialController : MonoBehaviour
    {
        public event Action<StepData> OnStepChanged;
        public event Action OnTutorialCompleted;
        
        [Inject] private TutorialDataController _tutorialDataController;

        [Header("DEBUG")] 
        [SerializeField] private bool skipTutorial;
        [Space]
        [SerializeField] private MainMenuCamera mainMenuCamera;
        [Header("Character Window")]
        [SerializeField] private Button openCharacterWindowButton;
        [SerializeField] private Button closeCharacterWindowButton;
        [SerializeField] private Button openSkillsResearchingPanel;
        [SerializeField] private Button closeSkillsResearchingPanel;
        [Header("Squads Manager")]
        [SerializeField] private Button openSquadsManagerButton;
        [SerializeField] private Button closeSquadsManagerButton;
        [Header("Undeads Window")]
        [SerializeField] private Button openUndeadsWindowButton;
        [SerializeField] private Button upgradeUndeadButton;
        [SerializeField] private Button closeMoreInformationButton;
        [SerializeField] private Button closeUndeadsWindowButton;
        [Header("Tasks Window")]
        [SerializeField] private Button openTaskWindowButton;
        [SerializeField] private Button closeTaskWindowButton;
        [Header("Objects to hide")]
        [SerializeField] private GameObject[] objectsToHide;
        [Space]
        [SerializeField] private List<TutorialStep> tutorialSteps;
        
        private int _currentStep;
#if UNITY_EDITOR
        public bool SkipTutorial => skipTutorial;
#endif
        public void Initialize()
        {
#if UNITY_EDITOR
            if (skipTutorial)
                return;
#endif
            
            if (_tutorialDataController.MainMenuTutorialCompleted)
                return;
            
            openCharacterWindowButton.interactable = false;
            closeCharacterWindowButton.interactable = false;
            openSkillsResearchingPanel.interactable = false;
            closeSkillsResearchingPanel.interactable = false;
            openTaskWindowButton.interactable = false;
            closeTaskWindowButton.interactable = false;
            
            openSquadsManagerButton.interactable = false;
            closeSquadsManagerButton.interactable = false;
            
            openUndeadsWindowButton.interactable = false;
            upgradeUndeadButton.interactable = false;
            closeMoreInformationButton.interactable = false;
            closeUndeadsWindowButton.interactable = false;
            
            mainMenuCamera.LockCamera();

            for (var i = 0; i < objectsToHide.Length; i++)
            {
                objectsToHide[i].gameObject.SetActive(false);
            }
            
            StartTutorialStep(0);
        }
        
        private void StartTutorialStep(int stepIndex)
        {
            _currentStep = stepIndex;
            
            if (stepIndex > tutorialSteps.Count - 1)
            {
                CompleteTutorial();
                return;
            }

            SaveTutorial();
            OnStepChanged?.Invoke(tutorialSteps[stepIndex].StepData);
            tutorialSteps[stepIndex].StepComplete += PerformStepComplete;
            tutorialSteps[stepIndex].StartStep();
        }
        
        private void CompleteTutorial()
        {
            openCharacterWindowButton.interactable = true;
            closeCharacterWindowButton.interactable = true;
            openSkillsResearchingPanel.interactable = true;
            closeSkillsResearchingPanel.interactable = true;
            openTaskWindowButton.interactable = true;
            closeTaskWindowButton.interactable = true;
            
            openSquadsManagerButton.interactable = true;
            closeSquadsManagerButton.interactable = true;
            
            openUndeadsWindowButton.interactable = true;
            closeMoreInformationButton.interactable = true;
            closeUndeadsWindowButton.interactable = true;
            
            mainMenuCamera.UnlockCamera();
            
            for (var i = 0; i < objectsToHide.Length; i++)
            {
                objectsToHide[i].gameObject.SetActive(true);
            }

            _tutorialDataController.MainMenuTutorialCompleted = true;
            OnTutorialCompleted?.Invoke();
        }
        
        private void SaveTutorial()
        {
            _tutorialDataController.MainMenuTutorialCompleted = false;
        }

        private void PerformStepComplete()
        {
            _currentStep++;
            StartTutorialStep(_currentStep);
        }
    }
}