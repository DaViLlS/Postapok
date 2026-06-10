using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.Scripts.Steps
{
    public class OpenSquadsPanelStep : TutorialStep
    {
        [SerializeField] private Button openSquadsPanelButton;
        [SerializeField] private GameObject arrowSign;
            
        public override void StartStep()
        {
            base.StartStep();
            
            arrowSign.SetActive(true);
            openSquadsPanelButton.onClick.AddListener(CompleteStep);
            openSquadsPanelButton.gameObject.SetActive(true);
        }

        public override void CompleteStep()
        {
            arrowSign.SetActive(false);
            openSquadsPanelButton.onClick.RemoveListener(CompleteStep);
            
            base.CompleteStep();
        }
    }
}