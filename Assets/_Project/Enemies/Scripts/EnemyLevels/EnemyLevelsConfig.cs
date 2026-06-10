using System.Collections.Generic;
using System.Linq;
using _Project.Enemies.Scripts;
using UnityEngine;

namespace _Project.Levels.Scripts.EnemyLevels
{
    [CreateAssetMenu(fileName = "Enemies Levels Config", menuName = "Enemies/Enemies Levels Config")]
    public class EnemyLevelsConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyLevelConfig> enemiesLevels;

        public EnemyLevelConfig GetEnemyLevelConfig(EnemyType enemyType)
        {
            return enemiesLevels.First(x => x.enemyType == enemyType);
        }
    }
}