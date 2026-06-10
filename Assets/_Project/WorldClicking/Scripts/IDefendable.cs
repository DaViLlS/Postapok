using UnityEngine;

namespace _Project.WorldClicking.Scripts
{
    public interface IDefendable
    {
        public GameObject GetGameObject();
        public Vector2 GetPosition();
    }
}