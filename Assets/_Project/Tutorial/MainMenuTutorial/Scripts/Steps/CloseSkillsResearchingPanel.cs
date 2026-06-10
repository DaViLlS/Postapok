using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class CloseSkillsResearchingPanel : TutorialStep
    {
        [SerializeField] private Button closeSkillsResearchingButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            closeSkillsResearchingButton.onClick.AddListener(SquadsWindowOpened);
            closeSkillsResearchingButton.interactable = true;
        }

        private void SquadsWindowOpened()
        {
            arrowSignObject.SetActive(false);
            closeSkillsResearchingButton.onClick.RemoveListener(SquadsWindowOpened);
            closeSkillsResearchingButton.interactable = false;
            CompleteStep();
        }
    }
}