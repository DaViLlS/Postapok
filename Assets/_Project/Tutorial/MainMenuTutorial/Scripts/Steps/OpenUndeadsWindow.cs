using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class OpenUndeadsWindow : TutorialStep
    {
        [SerializeField] private Button undeadsWindowButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            undeadsWindowButton.onClick.AddListener(SquadsWindowOpened);
            undeadsWindowButton.interactable = true;
        }

        private void SquadsWindowOpened()
        {
            arrowSignObject.SetActive(false);
            undeadsWindowButton.onClick.RemoveListener(SquadsWindowOpened);
            undeadsWindowButton.interactable = false;
            CompleteStep();
        }
    }
}