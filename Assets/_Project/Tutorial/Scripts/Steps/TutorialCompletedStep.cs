using System.Collections;
using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class TutorialCompletedStep : TutorialStep
    {
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private float completeTimeInSeconds;
        
        public override void StartStep()
        {
            base.StartStep();
            mainCharacter.Movement.LockMovement();
            StartCoroutine(CompleteStepRoutine());
        }

        private IEnumerator CompleteStepRoutine()
        {
            yield return new WaitForSeconds(completeTimeInSeconds);
            mainCharacter.Movement.UnlockMovement();
            CompleteStep();
        }
    }
}