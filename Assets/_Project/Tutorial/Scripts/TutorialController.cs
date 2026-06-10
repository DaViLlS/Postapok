using System;
using System.Collections.Generic;
using _Project.MainCharacter.Scripts;
using UnityEngine;
using Zenject;

namespace _Project.Tutorial.Scripts
{
    public class TutorialController : MonoBehaviour
    {
        public event Action<StepData> OnStepChanged;
        public event Action OnTutorialCompleted;
        
        [Inject] private TutorialDataController _tutorialDataController;
        
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private List<TutorialStep> tutorialSteps;
        [SerializeField] private Movement movement;
        
        private int _currentStep;

        private void Start()
        {
            InitializeTutorial();
        }

        public void InitializeTutorial()
        {
            mainCharacter.EnableImmortality();
            movement.LockCameraStateChanging();
        }

        public void StartTutorial()
        {
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
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                CompleteTutorial();
            }
        }
#endif
        private void CompleteTutorial()
        {
            mainCharacter.DisableImmortality();
            _tutorialDataController.FirstTutorialCompleted = true;
            OnTutorialCompleted?.Invoke();
        }
        
        private void SaveTutorial()
        {
            _tutorialDataController.FirstTutorialCompleted = false;
        }

        private void PerformStepComplete()
        {
            _currentStep++;
            StartTutorialStep(_currentStep);
        }
    }
}
