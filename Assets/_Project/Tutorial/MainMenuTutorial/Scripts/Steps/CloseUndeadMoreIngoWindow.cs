using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class CloseUndeadMoreIngoWindow : TutorialStep
    {
        [SerializeField] private Button closeMoreInfoButton;
        [SerializeField] private GameObject arrowSignObject;

        public override void StartStep()
        {
            base.StartStep();
            
            arrowSignObject.SetActive(true);
            closeMoreInfoButton.onClick.AddListener(MoreInfoClosed);
            closeMoreInfoButton.interactable = true;
        }

        private void MoreInfoClosed()
        {
            arrowSignObject.SetActive(false);
            closeMoreInfoButton.onClick.RemoveListener(MoreInfoClosed);
            closeMoreInfoButton.interactable = false;
            CompleteStep();
        }
    }
}