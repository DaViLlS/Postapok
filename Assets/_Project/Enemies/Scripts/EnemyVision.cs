using System;
using System.Collections.Generic;
using _Project.MainCharacter.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Enemies.Scripts
{
    public class EnemyVision : MonoBehaviour
    {
        public event Action<IAttackable> OnAttackableDetected;
        public event Action<IAttackable> OnAttackableLost;

        [SerializeField] private Enemy enemy;
        [SerializeField] private LayerMask detectLayers;
        
        private List<IAttackable> _attackables = new List<IAttackable>();
        
        public bool HasAttackable => _attackables.Count > 0;
        public MainCharacterController MainCharacter { get; private set; }

        public IAttackable GetRandomAttackable()
        {
            return _attackables[Random.Range(0, _attackables.Count)];
        }

        public IAttackable GetNearestAttackable()
        {
            IAttackable nearestAttackable = GetFirstAttackable();

            if (nearestAttackable == null)
                return null;

            foreach (var attackable in _attackables)
            {
                if (attackable == null || attackable.IsKilled())
                    continue;

                if (Vector2.Distance(attackable.GetPosition(), transform.position) <
                    Vector2.Distance(nearestAttackable.GetPosition(), transform.position))
                {
                    nearestAttackable = attackable;
                }
            }
            
            return nearestAttackable;
        }

        public List<IAttackable> GetAmountOfNearestAttackables(int amount)
        {
            var nearestAttackables = new List<IAttackable>();

            for (var i = 0; i < _attackables.Count - 1; i++)
            {
                for (var j = 0; j < _attackables.Count - i - 1; j++)
                {
                    if (Vector2.Distance(transform.position, _attackables[j].GetPosition()) 
                        > Vector2.Distance(transform.position, _attackables[j + 1].GetPosition()))
                    {
                        (_attackables[j], _attackables[j + 1]) = (_attackables[j + 1], _attackables[j]);
                    }
                }
            }

            for (var i = 0; i < amount; i++)
            {
                if (_attackables.Count <= i)
                    break;
                
                if (Vector2.Distance(transform.position, _attackables[i].GetPosition()) <= enemy.DistanceToAttack)
                    nearestAttackables.Add(_attackables[i]);
            }
            
            return nearestAttackables;
        }

        public IAttackable GetFirstAttackable()
        {
            if (_attackables.Count == 0)
                return null;
            
            return _attackables[0];
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsObjectOnLayer(other.gameObject, detectLayers))
                return;

            if (other.gameObject.TryGetComponent(out MainCharacterController character))
            {
                MainCharacter = character;
            }

            if (other.gameObject.TryGetComponent(out IAttackable attackable))
            {
                if (!attackable.IsVisible())
                    return;

                attackable.OnHided += RemoveAttackable;
                attackable.OnKilled += RemoveAttackable;
                _attackables.Add(attackable);
                OnAttackableDetected?.Invoke(attackable);
            }
        }

        private void RemoveAttackable(IAttackable attackable)
        {
            attackable.OnHided -= RemoveAttackable;
            attackable.OnKilled -= RemoveAttackable;
            _attackables.Remove(attackable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsObjectOnLayer(other.gameObject, detectLayers))
                return;
            
            if (other.gameObject.TryGetComponent(out MainCharacterController character))
            {
                if (MainCharacter.IsKilled())
                    MainCharacter = null;
            }
            
            if (other.gameObject.TryGetComponent(out IAttackable attackable))
            {
                if (_attackables.Contains(attackable) && attackable.IsKilled())
                {
                    attackable.OnHided -= RemoveAttackable;
                    attackable.OnKilled -= RemoveAttackable;
                    _attackables.Remove(attackable);
                    OnAttackableLost?.Invoke(attackable);
                }
            }
        }
        
        private bool IsObjectOnLayer(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }
    }
}