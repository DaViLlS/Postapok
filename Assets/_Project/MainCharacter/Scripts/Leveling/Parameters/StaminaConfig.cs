using UnityEngine;

namespace _Project.MainCharacter.Scripts.Leveling.Parameters
{
    [CreateAssetMenu(fileName = "New Stamina Config", menuName = "Character Parameters/New Stamina Config")]
    public class StaminaConfig : ScriptableObject
    {
        [Header("Уровень")]
        public ulong maxLevel;
        
        [Header("Базовый параметр")] 
        public ulong baseStamina;
        
        [Header("Кривая роста")] 
        public float staminaK;
        public float staminaB;
        
        public ulong GetStaminaForLevel(ulong level)
        {
            return (ulong)(baseStamina * Mathf.Pow(1f + level / staminaB, staminaK));
        }
    }
}