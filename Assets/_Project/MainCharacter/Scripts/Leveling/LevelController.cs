using System;
using Zenject;

namespace _Project.MainCharacter.Scripts.Leveling
{
    public class LevelController
    {
        public event Action<ulong> OnExperienceAdded;
        public event Action OnLevelIncreased;
        
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private ParametersConfig _parametersConfig;

        public void AddExperience(ulong experienceToAdd)
        {
            if (_mainCharacterData.CurrentLevel >= _parametersConfig.maxLevel)
                return;
            
            var nextExperience = _parametersConfig.GetExperienceForLevel(_mainCharacterData.CurrentLevel + 1);

            _mainCharacterData.AddExperience(experienceToAdd, this);
            
            if (_mainCharacterData.Experience >= nextExperience)
            {
                _mainCharacterData.ClearExperience();
                _mainCharacterData.IncLevel();
                _mainCharacterData.InkLevelPoints();
                OnLevelIncreased?.Invoke();
                return;
            }
            
            OnExperienceAdded?.Invoke(experienceToAdd);
        }
    }
}