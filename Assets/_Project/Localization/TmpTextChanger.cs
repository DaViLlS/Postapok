using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Localization
{
    public class TmpTextChanger : MonoBehaviour
    {
        [Inject] private LocalizationController _localizationController;
        
        [SerializeField] private TMP_Text text;
        [SerializeField] private TextData[] textVariants;

        private void Start()
        {
            if (_localizationController == null)
                return;
            
            _localizationController.OnLanguageChanged += ChangeLanguage;

            ChangeLanguage();
        }

        private void OnDestroy()
        {
            if (_localizationController == null)
                return;
            
            _localizationController.OnLanguageChanged -= ChangeLanguage;
        }

        private void ChangeLanguage()
        {
            for (var i = 0; i < textVariants.Length; i++)
            {
                if (textVariants[i].language == _localizationController.ChosenLanguage)
                {
                    text.text = textVariants[i].text;
                }
            }
        }
    }
}