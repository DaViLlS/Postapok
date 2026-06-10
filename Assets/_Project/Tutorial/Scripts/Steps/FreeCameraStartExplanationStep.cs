using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class FreeCameraStartExplanationStep : TutorialStep
    {
        [SerializeField] private Movement movement;
        
        public override void StartStep()
        {
            base.StartStep();

            movement.UnlockCameraStateChanging();
            movement.OnCameraStateChanged += PerformCameraStateChanged;
        }

        private void PerformCameraStateChanged()
        {
            movement.OnCameraStateChanged -= PerformCameraStateChanged;
            movement.LockCameraStateChanging();
            CompleteStep();
        }
    }
}