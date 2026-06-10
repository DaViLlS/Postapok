using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class CloseUndeadsWindow : TutorialStep
    {
        [SerializeField] private Button closeWindowButton;
        [SerializeField] private GameObject arrowSignObject;

        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            closeWindowButton.interactable = true;
            closeWindowButton.onClick.AddListener(CompleteStep);
        }

        public override void CompleteStep()
        {
            arrowSignObject.SetActive(false);
            closeWindowButton.onClick.RemoveListener(CompleteStep);
            closeWindowButton.interactable = false;
            
            base.CompleteStep();
        }
    }
}