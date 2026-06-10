using System.Collections;
using System.Collections.Generic;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Enemies.Scripts.EnemiesPaths
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private int enemiesCount = 1;
        [SerializeField] private float timeToReturnInSec;
        [SerializeField] private float waitOnPathInSec;
        [SerializeField] private Transform startPoint;
        [SerializeField] private List<PathPoint> pathPoints;

        private Coroutine _waitBeforeReturnCoroutine;
        private Coroutine _waitOnPathCoroutine;
        private List<Enemy> _enemies = new List<Enemy>();
        private int _currentPointIndex;
        private bool _isPathStarted;
        
        public int EnemiesCount => enemiesCount;
        public Transform StartPoint => startPoint;

        public void AssignEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            enemy.OnKilled += StopWalkingOnPath;
            enemy.EnemyVision.OnAttackableDetected += PefrormAttackableDetected;
            enemy.EnemyVision.OnAttackableLost += PerformAttackableLost;
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.OnKilled -= StopWalkingOnPath;
                enemy.EnemyVision.OnAttackableDetected -= PefrormAttackableDetected;
                enemy.EnemyVision.OnAttackableLost -= PerformAttackableLost;
            }
        }

        private void PefrormAttackableDetected(IAttackable attackable)
        {
            if (_waitBeforeReturnCoroutine != null)
                StopCoroutine(_waitBeforeReturnCoroutine);
            
            StopWalkingOnPath(attackable);
        }
        
        private void PerformAttackableLost(IAttackable attackable)
        {
            if (_waitBeforeReturnCoroutine != null)
                StopCoroutine(_waitBeforeReturnCoroutine);
            
            _waitBeforeReturnCoroutine = StartCoroutine(WaitBeforeReturn());
        }

        private IEnumerator WaitBeforeReturn()
        {
            yield return new WaitForSeconds(timeToReturnInSec);
            
            StartPath();
        }

        private void StopWalkingOnPath(IAttackable attackable)
        {
            StopWalkingOnPath();
        }

        private void StopWalkingOnPath()
        {
            _isPathStarted = false;
            
            if (_waitOnPathCoroutine != null)
                StopCoroutine(_waitOnPathCoroutine);
            
            if (_waitBeforeReturnCoroutine != null)
                StopCoroutine(_waitBeforeReturnCoroutine);
        }

        public void StartPath()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy == null)
                    return;
                
                if (enemy.IsKilled())
                    return;
            
                enemy.NavMeshAgent.Agent.isStopped = false;
                enemy.NavMeshAgent.Agent.ResetPath();
                _isPathStarted = true;
            }
        }

        private void Update()
        {
            if (!_isPathStarted)
                return;

            foreach (var enemy in _enemies)
            {
                if (enemy == null || enemy.IsKilled())
                    continue;
            
                if (_currentPointIndex >= pathPoints.Count - 1)
                    _currentPointIndex = 0;
            
                enemy.NavMeshAgent.Agent.SetDestination(pathPoints[_currentPointIndex].Point.position);
                
                if (Vector2.Distance(enemy.transform.position,
                        pathPoints[_currentPointIndex].Point.position) < pathPoints[_currentPointIndex].DistanceToStop)
                {
                    _currentPointIndex++;
                    StopWalkingOnPath();
                    
                    if (_waitOnPathCoroutine != null)
                        StopCoroutine(_waitOnPathCoroutine);
                    
                    _waitOnPathCoroutine = StartCoroutine(WaitOnPath());
                    break;
                }
            }
        }

        private IEnumerator WaitOnPath()
        {
            yield return new WaitForSeconds(waitOnPathInSec);
            
            StartPath();
        }

        private void OnDrawGizmos()
        {
            for (var i = 0; i < pathPoints.Count - 1; i++)
            {
                Debug.DrawLine(pathPoints[i].Point.position, pathPoints[i + 1].Point.position, Color.red);
            }
        }
    }
}