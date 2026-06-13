using System;
using UnityEngine;

namespace _Project.Main.Animations
{
    public class AnimationsController : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        
        private string _currentTrigger;
        
        public Animator Animator => animator;
        
        public void TriggerAnimation(string trigger)
        {
            if (_currentTrigger != string.Empty || _currentTrigger != "")
                ResetTriggerAnimation(_currentTrigger);
            
            _currentTrigger = trigger;
            animator?.SetTrigger(trigger);
        }

        public void ResetTriggerAnimation(string trigger)
        {
            animator?.ResetTrigger(trigger);
        }

        public void UpdateSpeed(float speed)
        {
            animator?.SetFloat("Speed", speed);
        }

        public void UpdateRun(bool isRunning)
        {
            animator?.SetBool("IsRunning", isRunning);
        }

        public void UpdateDirection(float direction)
        {
            animator?.SetFloat("Direction", direction);
        }
    }
}