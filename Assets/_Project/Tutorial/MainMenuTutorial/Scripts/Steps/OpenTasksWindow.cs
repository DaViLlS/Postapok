using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class OpenTasksWindow : TutorialStep
    {
        [SerializeField] private Button tasksWindowButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            tasksWindowButton.onClick.AddListener(SquadsWindowOpened);
            tasksWindowButton.interactable = true;
        }

        private void SquadsWindowOpened()
        {
            arrowSignObject.SetActive(false);
            tasksWindowButton.onClick.RemoveListener(SquadsWindowOpened);
            tasksWindowButton.interactable = false;
            CompleteStep();
        }
    }
}