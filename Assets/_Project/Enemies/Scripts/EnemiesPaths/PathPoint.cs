using UnityEngine;

namespace _Project.Enemies.Scripts.EnemiesPaths
{
    public class PathPoint : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        [SerializeField] private float distanceToStop;

        public float DistanceToStop => distanceToStop;
        public Transform Point => _point;
    }
}