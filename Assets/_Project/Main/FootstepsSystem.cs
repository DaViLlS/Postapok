using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Main
{
    public class FootstepsSystem : MonoBehaviour
    {
        [Inject] private SoundsController _soundsController;
        
        [SerializeField] private FootstepsConfig footstepsConfig;
        [SerializeField] private AudioSource audioSource;
        
        private Dictionary<TileBase, AudioClip[]> _soundDictionary = new Dictionary<TileBase, AudioClip[]>();
    
        [System.Serializable]
        public class TileSoundPair
        {
            public TileBase tile;
            public AudioClip[] sounds;
        }
    
        private void Start()
        {
            _soundsController.OnFootstepVolumeChanged += ChangeVolume;
            audioSource.volume = _soundsController.FootstepsVolume;
            
            _soundDictionary = new Dictionary<TileBase, AudioClip[]>();
            
            foreach (var pair in footstepsConfig.TileSoundPairs)
            {
                _soundDictionary[pair.tile] = pair.sounds;
            }
        }

        private void OnDestroy()
        {
            _soundsController.OnFootstepVolumeChanged -= ChangeVolume;
        }

        private void ChangeVolume()
        {
            audioSource.volume = _soundsController.FootstepsVolume;
        }

        public void MakeFootstep(AnimationEvent evt)
        {
            if (evt.animatorClipInfo.weight > 0.5f)
            {
                var groundTilePos = TilemapDataContainer.Instance.GrassTilemap.WorldToCell(transform.position);

                var tile = GetCurrentTile(groundTilePos);
                PlayFootstepSound(tile);
            }
        }

        private TileBase GetCurrentTile(Vector3Int tilePos)
        {
            var grassTile = TilemapDataContainer.Instance.GrassTilemap.GetTile(tilePos);
            var roadTile = TilemapDataContainer.Instance.RoadTilemap.GetTile(tilePos);

            if (roadTile != null)
            {
                return roadTile;
            }

            if (grassTile != null)
            {
                return grassTile;
            }
            
            return null;
        }

        private void PlayFootstepSound(TileBase tile)
        {
            if (_soundDictionary.ContainsKey(tile))
            {
                var sounds = _soundDictionary[tile];
                
                if (sounds.Length > 0)
                {
                    var clip = sounds[Random.Range(0, sounds.Length)];
                    audioSource.PlayOneShot(clip);
                }
            }
        }
    }
}