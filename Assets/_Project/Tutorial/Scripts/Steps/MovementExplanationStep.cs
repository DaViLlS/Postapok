using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class MovementExplanationStep : TutorialStep
    {
        [SerializeField] private Movement movement;

        public override void StartStep()
        {
            base.StartStep();
            movement.OnMovementPerformed += CompleteStep;
        }

        public override void CompleteStep()
        {
            movement.OnMovementPerformed -= CompleteStep;
            base.CompleteStep();
        }
    }
}