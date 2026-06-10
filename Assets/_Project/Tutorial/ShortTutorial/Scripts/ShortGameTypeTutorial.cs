using _Project.Localization;
using _Project.Main.UiBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Tutorial.ShortTutorial.Scripts
{
    public class ShortGameTypeTutorial : GameScreen
    {
        [Inject] private ShortGameTutorialConfig _shortGameTutorialConfig;
        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private Image tutorialImage;
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private GameObject prevButton;
        [SerializeField] private GameObject nextButton;

        private int _currentStep = 0;
        private ShortGameTutorialConfig _currentTutorialConfig;

        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public override void Close()
        {
            base.Close();
            _currentStep = 0;
        }

        public void Setup(ShortGameTutorialConfig shortGameTutorialConfig)
        {
            _currentTutorialConfig = shortGameTutorialConfig;
            prevButton.gameObject.SetActive(false);
            var tutorialData = shortGameTutorialConfig.ShortGameTutorialInfos[_currentStep];
            tutorialImage.sprite = tutorialData.tutorialSprite;
            tutorialText.text = tutorialData.GetText(_localizationController.ChosenLanguage);
        }

        public void Setup()
        {
            _currentTutorialConfig = _shortGameTutorialConfig;
            prevButton.gameObject.SetActive(false);
            var tutorialData = _shortGameTutorialConfig.ShortGameTutorialInfos[_currentStep];
            tutorialImage.sprite = tutorialData.tutorialSprite;
            tutorialText.text = tutorialData.GetText(_localizationController.ChosenLanguage);
        }

        public void NextStep()
        {
            _currentStep++;

            if (_currentStep >= _currentTutorialConfig.ShortGameTutorialInfos.Count - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            
            prevButton.gameObject.SetActive(true);
            
            var tutorialData = _currentTutorialConfig.ShortGameTutorialInfos[_currentStep];
            tutorialImage.sprite = tutorialData.tutorialSprite;
            tutorialText.text = tutorialData.GetText(_localizationController.ChosenLanguage);
        }

        public void PreviousStep()
        {
            _currentStep--;

            if (_currentStep == 0)
            {
                prevButton.gameObject.SetActive(false);
            }
            
            nextButton.gameObject.SetActive(true);
            
            var tutorialData = _currentTutorialConfig.ShortGameTutorialInfos[_currentStep];
            tutorialImage.sprite = tutorialData.tutorialSprite;
            tutorialText.text = tutorialData.GetText(_localizationController.ChosenLanguage);
        }
    }
}