using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Enemies.Scripts
{
    [Serializable]
    public class EnemyWaveData
    {
        public float timeBeforeSpawn;
        public List<Transform> spawnPoints = new List<Transform>();
        public List<Enemy> enemyPrefabs = new List<Enemy>();
        public bool needSpawnBoss;
        public Transform bossSpawnPoint;
        public Enemy bossPrefab;

        public int GetEnemiesCount()
        {
            if (needSpawnBoss)
            {
                return spawnPoints.Count + 1;
            }
            
            return spawnPoints.Count;
        }
    }
}