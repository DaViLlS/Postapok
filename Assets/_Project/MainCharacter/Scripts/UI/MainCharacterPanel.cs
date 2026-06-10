using System;
using _Project.MainCharacter.Scripts.Leveling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.MainCharacter.Scripts.UI
{
    public class MainCharacterPanel : MonoBehaviour
    {
        [Inject] private ParametersConfig _parametersConfig;
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private LevelController _levelController;
        
        [SerializeField] private Image avatarImage;
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text whiteExperienceText;
        [SerializeField] private TMP_Text blackExperienceText;

        private void Start()
        {
            levelText.text = _mainCharacterData.CurrentLevel.ToString();
            
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
            {
                nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel);
            }
            
            fillImage.fillAmount = (float)_mainCharacterData.Experience / nextExperience;
            whiteExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            blackExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;

            _levelController.OnExperienceAdded += UpdateExperienceView;
            _levelController.OnLevelIncreased += UpdateLevelView;
        }

        private void OnDestroy()
        {
            _levelController.OnExperienceAdded -= UpdateExperienceView;
            _levelController.OnLevelIncreased -= UpdateLevelView;
        }
        
        private void UpdateLevelView()
        {
            levelText.text = _mainCharacterData.CurrentLevel.ToString();
            
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
            {
                nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel);
            }
            
            fillImage.fillAmount = (float)_mainCharacterData.Experience / nextExperience;
            whiteExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
            blackExperienceText.text = _mainCharacterData.Experience + "/" + nextExperience;
        }

        private void UpdateExperienceView(ulong obj)
        {
            levelText.text = _mainCharacterData.CurrentLevel.ToString();
            
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