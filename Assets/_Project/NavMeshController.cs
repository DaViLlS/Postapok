using NavMeshPlus.Components;
using UnityEngine;

namespace _Project.Environment.Scripts
{
    public class NavMeshController : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface2D;

        public void Initialize()
        {
            surface2D.BuildNavMesh();
        }

        public void UpdateNavMesh()
        {
            surface2D.UpdateNavMesh(surface2D.navMeshData);
        }
    }
}
