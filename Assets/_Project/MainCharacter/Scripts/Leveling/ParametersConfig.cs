using _Project.MainCharacter.Scripts.Leveling.Parameters;
using UnityEngine;

namespace _Project.MainCharacter.Scripts.Leveling
{
    [CreateAssetMenu(fileName = "New Parameters Config", menuName = "Character Parameters/New Parameters Config")]
    public class ParametersConfig : ScriptableObject
    {
        [Header("Опыт")] 
        public float maxLevel;

        public ulong baseExperience;

        [Header("Кривая опыта")] 
        public float experienceK;
        public float experienceB;

        public ulong GetExperienceForLevel(ulong level)
        {
            return (ulong)Mathf.RoundToInt(100f * Mathf.Pow(1f + level / experienceB, experienceK));
        }
        
        [Header("Конфиги параметров")]
        public HealthConfig healthConfig;
        public StaminaConfig staminaConfig;
        public ManaConfig manaConfig;
    }
}