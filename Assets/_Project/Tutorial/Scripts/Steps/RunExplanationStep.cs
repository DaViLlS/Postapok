using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class RunExplanationStep : TutorialStep
    {
        [SerializeField] private Movement movement;

        public override void StartStep()
        {
            base.StartStep();
            movement.OnSprintPerformed += CompleteStep;
        }

        public override void CompleteStep()
        {
            movement.OnSprintPerformed -= CompleteStep;
            base.CompleteStep();
        }
    }
}