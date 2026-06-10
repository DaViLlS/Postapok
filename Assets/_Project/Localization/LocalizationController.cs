using System;

namespace _Project.Localization
{
    public class LocalizationController
    {
        public event Action OnLanguageChanged; 
        
        public LanguageType ChosenLanguage { get; private set; }

        public void Setup()
        {
            var language = LanguageType.Russian;
            
            ChosenLanguage = language;
            
            OnLanguageChanged?.Invoke();
        }
        
        public void Setup(LanguageType language)
        {
            ChosenLanguage = language;
            
            OnLanguageChanged?.Invoke();
        }
    }
}