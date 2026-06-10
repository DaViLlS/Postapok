using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class OpenSkillsResearchingPanel : TutorialStep
    {
        [SerializeField] private Button openSkillsResearchingButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            openSkillsResearchingButton.onClick.AddListener(SquadsWindowOpened);
            openSkillsResearchingButton.interactable = true;
        }

        private void SquadsWindowOpened()
        {
            arrowSignObject.SetActive(false);
            openSkillsResearchingButton.onClick.RemoveListener(SquadsWindowOpened);
            openSkillsResearchingButton.interactable = false;
            CompleteStep();
        }
    }
}