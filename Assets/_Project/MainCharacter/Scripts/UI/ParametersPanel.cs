using System;
using _Project.MainCharacter.Scripts.Leveling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.MainCharacter.Scripts.UI
{
    public class ParametersPanel : MonoBehaviour
    {
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private ParametersConfig _parametersConfig;
        [Inject] private LevelController _levelController;
        
        [SerializeField] private TMP_Text currentPointsCount;
        [SerializeField] private TMP_Text levelCount;
        [SerializeField] private TMP_Text experienceCount;
        [SerializeField] private TMP_Text healthCount;
        [SerializeField] private TMP_Text staminaCount;
        [SerializeField] private TMP_Text manaCount;
        [Space]
        [SerializeField] private Button healthUpgradeButton;
        [SerializeField] private Button staminaUpgradeButton;
        [SerializeField] private Button manaUpgradeButton;

        public void Initialize()
        {
            _levelController.OnExperienceAdded += UpdateExperienceCount;
            _levelController.OnLevelIncreased += UpdateView;
            
            var experienceToNextLevel = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            currentPointsCount.text = _mainCharacterData.LevelPoints.ToString();
            levelCount.text = _mainCharacterData.CurrentLevel.ToString();
            experienceCount.text = _mainCharacterData.Experience + "/" + experienceToNextLevel;
            healthCount.text = _parametersConfig.healthConfig.GetHealthForLevel(_mainCharacterData.CurrentHealthLevel).ToString();
            staminaCount.text = _parametersConfig.staminaConfig.GetStaminaForLevel(_mainCharacterData.CurrentStaminaLevel).ToString();

            if (_mainCharacterData.LevelPoints > 0)
            {
                if (_mainCharacterData.CurrentHealthLevel < _parametersConfig.healthConfig.maxLevel)
                    healthUpgradeButton.gameObject.SetActive(true);
                
                if (_mainCharacterData.CurrentStaminaLevel < _parametersConfig.staminaConfig.maxLevel)
                    staminaUpgradeButton.gameObject.SetActive(true);
            }
            else
            {
                healthUpgradeButton.gameObject.SetActive(false);
                staminaUpgradeButton.gameObject.SetActive(false);
                manaUpgradeButton.gameObject.SetActive(false);
            }
        }

        public void Dispose()
        {
            _levelController.OnExperienceAdded -= UpdateExperienceCount;
            _levelController.OnLevelIncreased -= UpdateView;
        }

        private void UpdateExperienceCount(ulong experience)
        {
            var experienceToNextLevel = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            experienceCount.text = _mainCharacterData.Experience + "/" + experienceToNextLevel;
        }

        private void UpdateView()
        {
            var experienceToNextLevel = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);
            
            experienceCount.text = _mainCharacterData.Experience + "/" + experienceToNextLevel;
            currentPointsCount.text = _mainCharacterData.LevelPoints.ToString();
            levelCount.text = _mainCharacterData.CurrentLevel.ToString();
            
            if (_mainCharacterData.LevelPoints > 0)
            {
                if (_mainCharacterData.CurrentHealthLevel < _parametersConfig.healthConfig.maxLevel)
                    healthUpgradeButton.gameObject.SetActive(true);
                
                if (_mainCharacterData.CurrentStaminaLevel < _parametersConfig.staminaConfig.maxLevel)
                    staminaUpgradeButton.gameObject.SetActive(true);
            }
        }

        public void UpgradeHealth()
        {
            _mainCharacterData.IncHealthLevel();
            _mainCharacterData.DecLevelPoints();
            
            healthCount.text = _parametersConfig.healthConfig.GetHealthForLevel(_mainCharacterData.CurrentHealthLevel).ToString();
            
            if (_mainCharacterData.CurrentHealthLevel >= _parametersConfig.healthConfig.maxLevel)
                healthUpgradeButton.gameObject.SetActive(false);
            
            currentPointsCount.text = _mainCharacterData.LevelPoints.ToString();

            if (_mainCharacterData.LevelPoints <= 0)
            {
                healthUpgradeButton.gameObject.SetActive(false);
                staminaUpgradeButton.gameObject.SetActive(false);
                manaUpgradeButton.gameObject.SetActive(false);
            }
        }

        public void UpgradeStamina()
        {
            _mainCharacterData.IncStaminaLevel();
            _mainCharacterData.DecLevelPoints();
            
            staminaCount.text = _parametersConfig.staminaConfig.GetStaminaForLevel(_mainCharacterData.CurrentStaminaLevel).ToString();
            
            if (_mainCharacterData.CurrentStaminaLevel >= _parametersConfig.staminaConfig.maxLevel)
                staminaUpgradeButton.gameObject.SetActive(false);
            
            currentPointsCount.text = _mainCharacterData.LevelPoints.ToString();
            
            if (_mainCharacterData.LevelPoints <= 0)
            {
                healthUpgradeButton.gameObject.SetActive(false);
                staminaUpgradeButton.gameObject.SetActive(false);
                manaUpgradeButton.gameObject.SetActive(false);
            }
        }
    }
}