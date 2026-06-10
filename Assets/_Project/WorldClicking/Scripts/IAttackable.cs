using System;
using UnityEngine;

namespace _Project.WorldClicking.Scripts
{
    public interface IAttackable
    {
        public event Action<IAttackable> OnKilled;
        public event Action<IAttackable> OnDestroyed;
        public event Action<IAttackable> OnVisibled;
        public event Action<IAttackable> OnHided;

        public bool IsKilled();
        public bool IsVisible();
        public void ApplyDamage(float damage);
        public bool CanApplyDamage();
        public Vector2 GetPosition();
        public Vector2 GetPositionWithOffset();
        public GameObject GetGameObject();
    }
}