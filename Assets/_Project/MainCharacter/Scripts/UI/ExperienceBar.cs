using _Project.MainCharacter.Scripts.Leveling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.MainCharacter.Scripts.UI
{
    public class ExperienceBar : MonoBehaviour
    {
        [Inject] private ParametersConfig _parametersConfig;
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private LevelController _levelController;
        
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text whiteExperienceText;
        [SerializeField] private TMP_Text blackExperienceText;

        private void Start()
        {
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
            {
                nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel);
            }
            
            fillImage.fillAmount = (float)_mainCharacterData.Experience / nextExperience;
            whiteExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            blackExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            
            levelText.text = _mainCharacterData.CurrentLevel.ToString();

            _levelController.OnLevelIncreased += IncreaseLevel;
            _levelController.OnExperienceAdded += ChangeExperience;
        }

        private void OnDestroy()
        {
            _levelController.OnLevelIncreased -= IncreaseLevel;
            _levelController.OnExperienceAdded -= ChangeExperience;
        }

        private void IncreaseLevel()
        {
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
            {
                nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel);
            }
            
            levelText.text = _mainCharacterData.CurrentLevel.ToString();
            fillImage.fillAmount = (float)_mainCharacterData.Experience / nextExperience;
            whiteExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            blackExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
        }
        
        private void ChangeExperience(ulong experience)
        {
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
            {
                nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel);
            }
            
            fillImage.fillAmount = (float)_mainCharacterData.Experience / nextExperience;
            whiteExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            blackExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
        }
    }
}