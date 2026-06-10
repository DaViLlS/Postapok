using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class FreeCameraReturnStep : TutorialStep
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
            CompleteStep();
        }
    }
}