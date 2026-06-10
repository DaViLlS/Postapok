using _Project.Enemies.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Main.GameManagement
{
    public class SoftValueCounter
    {
        private EnemiesManager _enemiesManager;
        
        public ulong SupportSoftValueCount { get; private set; }
        public ulong EnemyKilledSoftValueCount { get; private set; }
        public ulong BossKilledSoftValueCount { get; private set; }
        
        public SoftValueCounter(EnemiesManager enemiesManager)
        {
            _enemiesManager = enemiesManager;

            _enemiesManager.OnSupportEnded += AddSupportSftValue;
            _enemiesManager.OnEnemyKilled += AddEnemyKilledSoftValue;
            _enemiesManager.OnBossKilled += AddBossKilledSoftValue;
        }

        private void AddSupportSftValue()
        {
            SupportSoftValueCount += 20;
        }

        private void AddEnemyKilledSoftValue()
        {
            EnemyKilledSoftValueCount += 5;
        }

        private void AddBossKilledSoftValue()
        {
            BossKilledSoftValueCount += 50;
        }

        public ulong GetTotalSoftValueCount()
        {
            return SupportSoftValueCount + EnemyKilledSoftValueCount + BossKilledSoftValueCount;
        }
    }
}