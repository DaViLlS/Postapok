using System;
using System.Collections.Generic;
using _Project.Enemies.Scripts.EnemiesPaths;
using _Project.Levels.Scripts.EnemyLevels;
using _Project.Main.GameManagement;
using _Project.MainCharacter.Scripts;
using _Project.MainCharacter.Scripts.Leveling;
using _Project.TasksAndDialogues.Tasks.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Enemies.Scripts
{
    public class EnemiesManager : MonoBehaviour
    {
        public event Action OnSupportStarted;
        public event Action<float> OnTimerChanged; 
        public event Action OnSupportEnded;
        public event Action OnEnemyKilled;
        public event Action OnBossKilled;
        
        [Inject] private IInstantiator _instantiator;
        [Inject] private EnemyLevelsConfig _enemyLevelsConfig;
        [Inject] private TasksController _tasksController;
        [Inject] private LevelController _levelController;
        [Inject] private WorldClickingController _worldClickingController;
        
        [SerializeField] private GameLevelController gameController;
        [SerializeField] private Transform enemiesContainer;
        [SerializeField] private int enemiesTriggersCountToAlert;
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private float minDistanceBetweenObjects = 0.5f;
        [SerializeField] private float spawnRadius = 2f;
        [SerializeField] private int maxAttempts = 50;
        [SerializeField] private LayerMask checkSpawnLayers;
        [Space]
        [SerializeField] private Transform bossSpawnPoint;
        [SerializeField] private Enemy bossPrefab;
        [Space]
        [SerializeField] private List<Path> startEnemyPaths;
        [SerializeField] private List<Enemy> startEnemyPrefabs;
        [Space]
        [SerializeField] private List<Enemy> startCultistsPrefabs;
        [Space]
        [SerializeField] private List<EnemyWaveData> enemyWaves;

        private List<Enemy> _enemies = new List<Enemy>();

        private EnemyWaveData _currentWaveData;
        private Coroutine _waveCooldownCoroutine;
        
        public List<Enemy> Enemies => _enemies;
        

        public void Initialize()
        {
            
        }

        private void OnDestroy()
        { 
            
        }
        
        private void DestroyAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.OnKilled -= PerformEnemyKilled;
                enemy.Destroy();
            }
        }

        public void SpawnStartEnemies()
        {
            foreach (var startEnemyPath in startEnemyPaths)
            {
                for (var i = 0; i < startEnemyPath.EnemiesCount; i++)
                {
                    var randomEnemyPrefabIndex = Random.Range(0, startEnemyPrefabs.Count);
                
                    SpawnEnemy(startEnemyPrefabs[randomEnemyPrefabIndex], startEnemyPath);
                }
                
                startEnemyPath.StartPath();
            }

        }

        public void SpawnEnemy(Enemy enemyPrefab, Path path)
        {
            var spawnPosition = path.StartPoint.transform.position;
            var validPosition = false;
            var attempts = 0;
            
            while (!validPosition && attempts < maxAttempts)
            {
                var randomOffset = Random.insideUnitCircle * spawnRadius;
                spawnPosition = (Vector2)path.StartPoint.transform.position + randomOffset;
            
                var colliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceBetweenObjects, checkSpawnLayers);
            
                if (colliders.Length == 0)
                {
                    validPosition = true;
                }
            
                attempts++;
            }
            
            var enemy = _instantiator.InstantiatePrefabForComponent<Enemy>(enemyPrefab, spawnPosition, Quaternion.identity, path.StartPoint);
            enemy.OnKilled += PerformEnemyKilled;
            enemy.transform.SetParent(path.transform);
            path.AssignEnemy(enemy);
            var randomLevel = (ulong)Random.Range(1, 2);
            var enemyLevelConfig = _enemyLevelsConfig.GetEnemyLevelConfig(enemy.EnemyType);
            enemy.Initialize(enemyLevelConfig, randomLevel);
            _enemies.Add(enemy);
        }

        public Enemy SpawnEnemy(Enemy enemyPrefab, Transform spawnPoint)
        {
            var spawnPosition = spawnPoint.position;
            var validPosition = false;
            var attempts = 0;
            
            while (!validPosition && attempts < maxAttempts)
            {
                var randomOffset = Random.insideUnitCircle * spawnRadius;
                spawnPosition = (Vector2)spawnPoint.position + randomOffset;
            
                var colliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceBetweenObjects, checkSpawnLayers);
            
                if (colliders.Length == 0)
                {
                    validPosition = true;
                }
            
                attempts++;
            }
            
            var enemy = _instantiator.InstantiatePrefabForComponent<Enemy>(enemyPrefab, spawnPosition, Quaternion.identity, enemiesContainer);
            enemy.OnKilled += PerformEnemyKilled;
            var randomLevel = (ulong)Random.Range(1, 2);
            var enemyLevelConfig = _enemyLevelsConfig.GetEnemyLevelConfig(enemy.EnemyType);
            enemy.Initialize(enemyLevelConfig, randomLevel);
            _enemies.Add(enemy);

            return enemy;
        }
        
        private void PerformEnemyKilled(IAttackable attackable)
        {
            attackable.OnKilled -= PerformEnemyKilled;
            
            OnEnemyKilled?.Invoke();
        }
    }
}