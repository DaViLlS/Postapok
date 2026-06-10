using System;
using _Project.Main.PlayerData;
using _Project.MainCharacter.Scripts.Leveling;

namespace _Project.MainCharacter.Scripts
{
    public class MainCharacterData
    {
        public event Action OnSoftValueChanged;
        public event Action OnHardValueChanged;
        
        public bool HasPet { get; private set; }
        public ulong LevelPoints { get; private set; }
        public ulong Experience { get; private set; }
        public ulong CurrentLevel { get; private set; }
        public ulong CurrentHealthLevel { get; private set; }
        public ulong CurrentStaminaLevel { get; private set; }
        public ulong SoftValue { get; private set; }
        public ulong HardValue { get; private set; }
        public DateTime LastSessionTime { get; private set; }

        public void Initialize()
        {
            HasPet = false;
            LevelPoints = 0;
            Experience = 0;
            CurrentLevel = 1;
            CurrentHealthLevel = 1;
            CurrentStaminaLevel = 1;
            SoftValue = 0;
            HardValue = 0;
        }

        public void Load(PlayerSaveData data)
        {
            HasPet = data.hasPet;
            LevelPoints = data.levelPoints;
            Experience = data.experience;
            CurrentLevel = data.currentLevel;
            CurrentHealthLevel = data.currentHealthLevel;
            CurrentStaminaLevel = data.currentStaminaLevel;
            SoftValue = data.softValue;
            HardValue = data.hardValue;
            LastSessionTime = data.lastSessionTime;
        }

        public void UnlockPet()
        {
            HasPet = true;
        }

        public void AddExperience(ulong experience, LevelController levelController)
        {
            Experience += experience;
        }

        public void ClearExperience()
        {
            Experience = 0;
        }

        public void IncLevel()
        {
            CurrentLevel++;
        }

        public void InkLevelPoints()
        {
            LevelPoints++;
        }

        public void DecLevelPoints()
        {
            LevelPoints--;
        }

        public void IncHealthLevel()
        {
            CurrentHealthLevel++;
        }

        public void IncStaminaLevel()
        {
            CurrentStaminaLevel++;
        }

        public void AddSoftValue(ulong softValue)
        {
            SoftValue += softValue;
            OnSoftValueChanged?.Invoke();
        }

        public void RemoveSoftValue(ulong softValue)
        {
            SoftValue -= softValue;
            OnSoftValueChanged?.Invoke();
        }
        
        public void AddHardValue(ulong hardValue)
        {
            HardValue += hardValue;
            OnHardValueChanged?.Invoke();
        }

        public void RemoveHardValue(ulong hardValue)
        {
            HardValue -= hardValue;
            OnHardValueChanged?.Invoke();
        }

        public bool TryRemoveSoftValue(ulong softValue)
        {
            if (SoftValue < softValue)
            {
                return false;
            }
            
            SoftValue -= softValue;

            return true;
        }
    }
}