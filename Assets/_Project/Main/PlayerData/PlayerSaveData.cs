using System;
using _Project.MainCharacter.Scripts;

namespace _Project.Main.PlayerData
{
    [Serializable]
    public class PlayerSaveData
    {
        public bool hasPet;
        public ulong levelPoints;
        public ulong experience;
        public ulong currentLevel;
        public ulong currentHealthLevel;
        public ulong currentStaminaLevel;
        public ulong softValue;
        public DateTime lastSessionTime;

        public void Setup(MainCharacterData mainCharacterData)
        {
            hasPet = mainCharacterData.HasPet;
            levelPoints = mainCharacterData.LevelPoints;
            experience = mainCharacterData.Experience;
            currentLevel = mainCharacterData.CurrentLevel;
            currentHealthLevel = mainCharacterData.CurrentHealthLevel;
            currentStaminaLevel = mainCharacterData.CurrentStaminaLevel;
            softValue = mainCharacterData.SoftValue;
            lastSessionTime = DateTime.Now;
        }
    }
}