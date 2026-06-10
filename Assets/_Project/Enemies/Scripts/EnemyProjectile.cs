using System;
using _Project.MainCharacter.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Enemies.Scripts
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        
        private IAttackable _attackable;
        private float _damage;
        private float _speed;
        private Vector2 _currentVelocity;
        private bool _attackableKilled;

        private bool _isReady;
        
        public void Setup(IAttackable attackable, float damage, float speed)
        {
            _attackable = attackable;

            _attackable.OnKilled += PerformAttackableKilled;
            
            _damage = damage;
            _speed = speed;
            
            _isReady = true;
        }

        private void OnDestroy()
        {
            if (_attackable != null)
            {
                _attackable.OnKilled -= PerformAttackableKilled;
            }
        }

        private void PerformAttackableKilled(IAttackable attackable)
        {
            _attackableKilled = true;
        }

        private void FixedUpdate()
        {
            if (!_isReady)
                return;

            var targetPosition = Vector2.zero;

            if (_attackable != null && !_attackableKilled)
            {
                targetPosition = _attackable.GetPositionWithOffset();
            }
            
            var direction = targetPosition - (Vector2)transform.position;
            var normalizedDirection = direction.normalized;
            rb.linearVelocity = normalizedDirection * _speed;
            
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                var angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IAttackable>(out var attackable))
            {
                if (_attackable != null && _attackable == attackable && !_attackableKilled)
                {
                    _attackable.ApplyDamage(_damage);
                    Destroy(gameObject);
                    return;
                }
            }
            
            var attackble = other.GetComponentInParent<IAttackable>();

            if (_attackable != null && _attackable == attackble)
            {
                _attackable.ApplyDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}