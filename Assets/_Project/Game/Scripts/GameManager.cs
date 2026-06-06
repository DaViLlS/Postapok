using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Game.Scripts
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private Transform[] spawnPoints;
        
        [Header("Game Settings")]
        [SerializeField] private bool enableFriendlyFire = false;
        
        private NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(0);
        
        // Словарь для отслеживания уже заспавненных игроков
        private Dictionary<ulong, bool> spawnedPlayers = new Dictionary<ulong, bool>();
        
        // События для UI
        public event Action<int> OnPlayersCountChanged;
        public event Action OnGameEnded;
        
        private void Start()
        {
            connectedPlayers.OnValueChanged += (old, New) =>
            {
                OnPlayersCountChanged?.Invoke(New);
            };
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
                
                // Спавним всех текущих игроков
                foreach (var client in NetworkManager.Singleton.ConnectedClients)
                {
                    SpawnPlayer(client.Key);
                }
            }
            else
            {
                // Клиент запрашивает спавн
                RequestSpawnServerRpc();
            }
        }
        
        private void OnClientConnected(ulong clientId)
        {
            connectedPlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
            SpawnPlayer(clientId);
            Debug.Log($"Player {clientId} joined. Total: {connectedPlayers.Value}");
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            connectedPlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
            Debug.Log($"Player {clientId} left. Total: {connectedPlayers.Value}");
            
            // Убираем из словаря при отключении
            if (spawnedPlayers.ContainsKey(clientId))
            {
                spawnedPlayers.Remove(clientId);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void RequestSpawnServerRpc(ServerRpcParams rpcParams = default)
        {
            SpawnPlayer(rpcParams.Receive.SenderClientId);
        }
        
        private void SpawnPlayer(ulong clientId)
        {
            // ПРОВЕРКА 1: Уже заспавнен в словаре
            if (spawnedPlayers.ContainsKey(clientId) && spawnedPlayers[clientId])
            {
                Debug.LogWarning($"Player {clientId} already spawned (dictionary check)");
                return;
            }
            
            // ПРОВЕРКА 2: Уже есть PlayerObject у клиента
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient))
            {
                if (networkClient.PlayerObject != null)
                {
                    Debug.LogWarning($"Player {clientId} already has PlayerObject");
                    return;
                }
            }
            
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("No spawn points assigned!");
                return;
            }
            
            // Отмечаем что игрок спавнится
            spawnedPlayers[clientId] = true;
            
            // Выбираем точку спавна
            Transform spawnPoint = spawnPoints[clientId % (ulong)spawnPoints.Length];
            
            // Спавним игрока
            GameObject playerObj = Instantiate(
                NetworkManager.Singleton.NetworkConfig.PlayerPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
            
            playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            
            Debug.Log($"Spawned player {clientId} at {spawnPoint.name}");
        }
        
        public Vector3 GetRandomSpawnPosition()
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
            }
            return Vector3.zero;
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            }
        }
    }
}