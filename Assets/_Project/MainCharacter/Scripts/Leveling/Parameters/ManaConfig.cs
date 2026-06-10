using UnityEngine;

namespace _Project.MainCharacter.Scripts.Leveling.Parameters
{
    [CreateAssetMenu(fileName = "New Mana Config", menuName = "Character Parameters/New Mana Config")]
    public class ManaConfig : ScriptableObject
    {
        [Header("Уровень")]
        public ulong maxLevel;
        
        [Header("Базовый параметр")] 
        public ulong baseMana;
        
        [Header("Кривая роста")] 
        public float manaK;
        public float manaB;
        
        public ulong GetManaForLevel(ulong level)
        {
            return (ulong)(baseMana * Mathf.Pow(1f + level / manaB, manaK));
        }
    }
}