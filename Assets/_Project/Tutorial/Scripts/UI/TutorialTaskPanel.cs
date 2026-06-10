using _Project.Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Tutorial.Scripts.UI
{
    public class TutorialTaskPanel : MonoBehaviour
    {
        [Inject] private TutorialController _tutorialController;
        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        private void Awake()
        {
            _tutorialController.OnStepChanged += UpdatePanel;
        }

        private void OnDestroy()
        {
            _tutorialController.OnStepChanged -= UpdatePanel;
        }

        private void UpdatePanel(StepData stepData)
        {
            title.text = stepData.GetTitle(_localizationController.ChosenLanguage);
            description.text = stepData.GetDescription(_localizationController.ChosenLanguage);
        }
    }
}