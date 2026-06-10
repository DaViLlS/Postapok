using System;
using UnityEngine;

namespace _Project.WorldClicking.Scripts
{
    public interface IDestroyable
    {
        public event Action<IDestroyable> OnDestroyed;
        
        public void ApplyDamage(float damage);
        public Vector2 GetPosition();
        public Vector2 GetPositionWithOffset();
        public GameObject GetGameObject();
    }
}