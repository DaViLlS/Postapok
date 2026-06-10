using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Tutorial.Scripts
{
    public abstract class TutorialStep : MonoBehaviour
    {
        public event Action StepComplete;

        [SerializeField] private StepData stepData;
        [SerializeField] private List<GameObject> bordersToOpen = new List<GameObject>();

        protected bool IsActive;

        public StepData StepData => stepData;
        
        public virtual void StartStep()
        {
            IsActive = true;
            Debug.Log("Step started");
        }

        public virtual void CompleteStep()
        {
            IsActive = false;

            foreach (var borderToOpen in bordersToOpen)
            {
                borderToOpen.SetActive(false);
            }
            
            Debug.Log("Step completed");
            StepComplete?.Invoke();
        }
    }
}