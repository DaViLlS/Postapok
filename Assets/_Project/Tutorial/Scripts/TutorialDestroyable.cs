using System;
using _Project.Stats;
using _Project.Stats.UI;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Tutorial.Scripts
{
    public class TutorialDestroyable : MonoBehaviour, IDestroyable
    {
        public event Action<IDestroyable> OnDestroyed;
        
        [SerializeField] private float health;
        [SerializeField] private HealthView healthView;
        [SerializeField] protected Vector2 positionOffset;
        
        private Health _health;

        private void Awake()
        {
            _health = new Health(health);
            _health.OnZeroHealth += Destroy;
            healthView.Initialize(_health);
            healthView.ChangeLockState(false);
        }

        private void OnDestroy()
        {
            _health.OnZeroHealth -= Destroy;
            Destroy(gameObject);
        }

        private void Destroy()
        {
            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public void ApplyDamage(float damage)
        {
            _health.Damage(damage);
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
        
        public Vector2 GetPositionWithOffset()
        {
            return (Vector2)transform.position + positionOffset;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}