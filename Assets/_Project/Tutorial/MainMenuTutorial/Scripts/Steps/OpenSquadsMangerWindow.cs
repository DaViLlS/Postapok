using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class OpenSquadsMangerWindow : TutorialStep
    {
        [SerializeField] private Button squadsWindowButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            squadsWindowButton.onClick.AddListener(SquadsWindowOpened);
            squadsWindowButton.interactable = true;
        }

        private void SquadsWindowOpened()
        {
            arrowSignObject.SetActive(false);
            squadsWindowButton.onClick.RemoveListener(SquadsWindowOpened);
            squadsWindowButton.interactable = false;
            CompleteStep();
        }
    }
}