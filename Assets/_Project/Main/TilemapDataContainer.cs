using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Main
{
    public class TilemapDataContainer : MonoBehaviour
    {
        [SerializeField] private Tilemap grassTilemap;
        [SerializeField] private Tilemap roadTilemap;
        [SerializeField] private Tilemap bridgeTilemap;
        
        public Tilemap GrassTilemap => grassTilemap;
        public Tilemap RoadTilemap => roadTilemap;
        public Tilemap BridgeTilemap => bridgeTilemap;
        
        public static TilemapDataContainer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}