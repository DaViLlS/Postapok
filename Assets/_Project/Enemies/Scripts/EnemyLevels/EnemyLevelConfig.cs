using _Project.Enemies.Scripts;
using UnityEngine;

namespace _Project.Levels.Scripts.EnemyLevels
{
    [CreateAssetMenu(fileName = "Enemy Level Config", menuName = "Enemies/Enemy Level Config")]
    public class EnemyLevelConfig : ScriptableObject
    {
        [Header("Идентификация")]
        public EnemyType enemyType;          // "Житель ближник"
        public EnemyArchetype archetype;  // Enum: TANK, FIGHTER, SHOOTER, SKIRMISHER, SUPPORT, SIEGE
    
        [Header("Появление")]
        public int firstMissionLevel;     // С какого номера миссии появляется (1, 8, 21...)
    
        [Header("Базовые параметры (уровень 1)")]
        public float baseHP;
        public float baseDamage;
        public float baseAttackCooldown;
        public float baseSpeed;
        public float baseAttackRange;
        public float baseRetreatRange;
        public int baseExpReward;         // Опыт за убийство
        public int chanceToSpawnSoul;
    
        [Header("Кривые роста")]
        public float hpK;
        public float hpB;
        public float damageK;
        public float damageB;
        public float cooldownK;           // Отрицательная
        public float cooldownB;
        // SPD не растёт с уровнем у обычных врагов
        public float expK;
        public float expB;
    
        [Header("Способность")]
        public string abilityName;        // "Нет", "Рывок", "Кровотечение"...
        public string abilityDescription; // Для подсказок в UI
    
        public EnemyStats GetStatsForLevel(ulong level)
        {
            return new EnemyStats
            {
                hp = baseHP * Mathf.Pow(1f + level / hpB, hpK),
                damage = baseDamage * Mathf.Pow(1f + level / damageB, damageK),
                attackCooldown = baseAttackCooldown * Mathf.Pow(1f + level / cooldownB, cooldownK),
                speed = baseSpeed,
                attackRange = baseAttackRange,
                retreatRange = baseRetreatRange
            };
        }
    
        public ulong GetExpForLevel(ulong level)
        {
            return (ulong)Mathf.RoundToInt(baseExpReward * Mathf.Pow(1f + level / expB, expK));
        }
    }
    
    [System.Serializable]
    public struct EnemyStats
    {
        public float hp;
        public float damage;
        public float attackCooldown;
        public float speed;
        public float attackRange;
        public float retreatRange;
    }

    public enum EnemyArchetype
    {
        Tank,
        Fighter,
        Shooter,
        Skirmisher,
        Support,
        Siege
    }
}