using _Project.Localization;
using _Project.Tutorial.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.UI
{
    public class MainMenuTutorialTaskPanel : MonoBehaviour
    {
        [Inject] private TutorialDataController _tutorialDataController;
        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private MainMenuTutorialController tutorialController;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        
        private StepData _currentStepData;

        private void Awake()
        {
#if UNITY_EDITOR
            if (tutorialController.SkipTutorial)
            {
                gameObject.SetActive(false);
                return;
            }
#endif

            if (_tutorialDataController.MainMenuTutorialCompleted)
            {
                gameObject.SetActive(false);
                return;
            }
            
            tutorialController.OnStepChanged += UpdatePanel;
            tutorialController.OnTutorialCompleted += DisablePanel;
            _localizationController.OnLanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            tutorialController.OnStepChanged -= UpdatePanel;
            tutorialController.OnTutorialCompleted -= DisablePanel;
            _localizationController.OnLanguageChanged -= ChangeLanguage;
        }

        private void DisablePanel()
        {
            gameObject.SetActive(false);
        }
        
        private void ChangeLanguage()
        {
            title.text = _currentStepData.GetTitle(_localizationController.ChosenLanguage);
            description.text = _currentStepData.GetDescription(_localizationController.ChosenLanguage);
        }

        private void UpdatePanel(StepData stepData)
        {
            _currentStepData = stepData;
            
            title.text = stepData.GetTitle(_localizationController.ChosenLanguage);
            description.text = stepData.GetDescription(_localizationController.ChosenLanguage);
        }
    }
}