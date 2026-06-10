using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.Scripts.Steps.ExplanationSteps
{
    public class ExplanationStep : TutorialStep
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private GameObject arrowObject;

        protected Coroutine TimeCoroutine { get; private set; }

        public override void StartStep()
        {
            base.StartStep();
            
            if (arrowObject != null)
                arrowObject.SetActive(true);
            
            TimeCoroutine = StartCoroutine(CompleteStepRoutine());
        }

        public override void CompleteStep()
        {
            if (arrowObject != null)
                arrowObject.SetActive(false);
            
            nextButton.onClick.RemoveListener(CompleteStep);
            nextButton.gameObject.SetActive(false);
            
            if (TimeCoroutine != null)
                StopCoroutine(TimeCoroutine);
            
            base.CompleteStep();
        }
        
        private IEnumerator CompleteStepRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(CompleteStep);
        }
    }
}