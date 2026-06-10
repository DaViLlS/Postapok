using UnityEngine;

namespace _Project.MainCharacter.Scripts.Leveling.Parameters
{
    [CreateAssetMenu(fileName = "New Health Config", menuName = "Character Parameters/New Health Config")]
    public class HealthConfig : ScriptableObject
    {
        [Header("Уровень")]
        public ulong maxLevel;
        
        [Header("Базовый параметр")] 
        public ulong baseHealth;
        
        [Header("Кривая роста")] 
        public float healthK;
        public float healthB;
        
        public ulong GetHealthForLevel(ulong level)
        {
            return (ulong)(baseHealth * Mathf.Pow(1f + level / healthB, healthK));
        }
    }
}