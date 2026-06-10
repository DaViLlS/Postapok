using _Project.CameraControlling;
using _Project.Main.UiBase;
using _Project.Tutorial.Scripts;
using UnityEngine;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class LookAtLevel : TutorialStep
    {
        [SerializeField] private MainMenuCamera mainMenuCamera;
        [SerializeField] private float distanceToComplete = 10f;
        [SerializeField] private Transform target;
        [SerializeField] private WindowPointer windowPointer;
        
        private bool _reachedTarget;
        
        public override void StartStep()
        {
            base.StartStep();
            windowPointer.SetTarget(target);
            windowPointer.EnablePointer();
            mainMenuCamera.UnlockCamera();
        }

        private void Update()
        {
            if (!IsActive)
                return;
            
            if (_reachedTarget)
                return;
            
            if (Vector2.Distance(mainMenuCamera.transform.position, target.position) <= distanceToComplete)
            {
                _reachedTarget = true;
                CompleteStep();
            }
        }

        public override void CompleteStep()
        {
            mainMenuCamera.LockCamera();
            windowPointer.DisablePointer();
            base.CompleteStep();
        }
    }
}