using System;
using System.Collections.Generic;
using _Project.Enemies.Scripts;
using UnityEngine;

namespace _Project.NPCs.Scripts
{
    public class NpcVision : MonoBehaviour
    {
        public event Action<Enemy> OnEnemyDetected;
        public event Action<Enemy> OnEnemyLost;
        
        [SerializeField] private LayerMask detectLayers;
        
        private List<Enemy> _enemies = new List<Enemy>();
        
        public bool HasEnemies => _enemies.Count > 0;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsObjectOnLayer(other.gameObject, detectLayers))
                return;

            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                _enemies.Add(enemy);
                OnEnemyDetected?.Invoke(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsObjectOnLayer(other.gameObject, detectLayers))
                return;
            
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (_enemies.Contains(enemy))
                {
                    _enemies.Remove(enemy);
                    OnEnemyLost?.Invoke(enemy);
                }
            }
        }
        
        private bool IsObjectOnLayer(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }
    }
}