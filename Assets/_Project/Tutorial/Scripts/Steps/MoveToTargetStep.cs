using _Project.Main.UiBase;
using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts.Steps
{
    public class MoveToTargetStep : TutorialStep
    {
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private WindowPointer pointer;
        [SerializeField] private Transform target;
        [SerializeField] private float targetDistance;

        public override void StartStep()
        {
            base.StartStep();
            
            pointer.SetTarget(target);
            pointer.EnablePointer();
        }

        private void Update()
        {
            if (!IsActive)
                return;
            
            if (Vector2.Distance(mainCharacter.transform.position, target.position) < targetDistance)
            {
                pointer.DisablePointer();
                CompleteStep();
            }
        }
    }
}