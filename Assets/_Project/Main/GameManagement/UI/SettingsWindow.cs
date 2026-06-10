using _Project.Localization;
using _Project.Main.SavingSystem;
using _Project.Main.UiBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Main.GameManagement.UI
{
    public class SettingsWindow : GameScreen
    {
        [Inject] private SoundsController _soundsController;
        [Inject] private LocalizationController _localizationController;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider effectsSlider;
        [SerializeField] private Slider footstepsSlider;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button authorizeButton;
        
        public override void Initialize()
        {
            musicSlider.value = _soundsController.MusicVolume;
            effectsSlider.value = _soundsController.EffectsVolume;
            footstepsSlider.value = _soundsController.FootstepsVolume;

            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            effectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
            footstepsSlider.onValueChanged.AddListener(ChangeFootstepsVolume);
            
            for (var i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == "Русский")
                {
                    if (_localizationController.ChosenLanguage == LanguageType.Russian)
                    {
                        dropdown.value = i;
                        break;
                    }
                }
                else if (dropdown.options[i].text == "English")
                {
                    if (_localizationController.ChosenLanguage == LanguageType.English)
                    {
                        dropdown.value = i;
                        break;
                    }
                }
            }
            
            dropdown.onValueChanged.AddListener(CheckValue);
        }

        private void UpdateData()
        {
            
        }

        private void ChangeMusicVolume(float volume)
        {
            _soundsController.ChangeMusicVolume(volume);
        }

        private void ChangeEffectsVolume(float volume)
        {
            _soundsController.ChangeEffectsVolume(volume);
        }

        private void ChangeFootstepsVolume(float volume)
        {
            _soundsController.ChangeFootstepsVolume(volume);
        }

        public override void Dispose()
        {
            musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
            effectsSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
            footstepsSlider.onValueChanged.RemoveListener(ChangeFootstepsVolume);
        }
        
        private void CheckValue(int arg)
        {
            if (dropdown.options[arg].text == "Русский")
            {
                ChangeLanguage(LanguageType.Russian);
            }
            else if (dropdown.options[arg].text == "English")
            {
                ChangeLanguage(LanguageType.English);
            }
        }

        public void ChangeLanguage(LanguageType language)
        {
            _localizationController.Setup(language);
        }

        public void ResetSaves()
        {
            Repository.ClearData();
        }
    }
}