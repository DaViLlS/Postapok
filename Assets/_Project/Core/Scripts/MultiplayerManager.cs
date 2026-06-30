using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Core.Scripts
{
    public class MultiplayerManager : MonoBehaviour
    {
        public event Action<string> OnStatusChanged;
        public event Action<string> OnLobbyCreated;
        public event Action<int> OnPlayersCountChanged;
        public event Action OnGameStarted;
        public event Action OnDisconnected;
        
        private string _joinCode;
        private bool _isHost;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private async void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            
            await InitializeServices();
        }
        
        private async Task InitializeServices()
        {
            try
            {
                SetStatus("Initializing services...");
                
                await UnityServices.InitializeAsync();
                
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
                
                SetStatus($"Ready! ID: {AuthenticationService.Instance.PlayerId[..8]}...");
            }
            catch (Exception e)
            {
                SetStatus($"Init failed: {e.Message}");
                Debug.LogError($"Service init error: {e}");
            }
        }
        
        public async Task StartHost()
        {
            try
            {
                SetStatus("Creating session...");
                _isHost = true;
                
                var allocation = await RelayService.Instance.CreateAllocationAsync(3);
                _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                
                var relayData = new RelayServerData(allocation, "dtls");
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayData);
                
                NetworkManager.Singleton.StartHost();
                
                OnLobbyCreated?.Invoke(_joinCode);
                SetStatus("Host created!");
            }
            catch (Exception e)
            {
                SetStatus($"Host failed: {e.Message}");
                Debug.LogError($"Host error: {e}");
            }
        }
        
        public async Task JoinGame(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                SetStatus("Enter join code!");
                return;
            }
            
            try
            {
                SetStatus("Joining...");
                _isHost = false;
                
                var allocation = await RelayService.Instance.JoinAllocationAsync(code);
                var relayData = new RelayServerData(allocation, "dtls");
                
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayData);
                
                NetworkManager.Singleton.StartClient();
                _joinCode = code;
                
                OnLobbyCreated?.Invoke(code);
                SetStatus("Connected!");
            }
            catch (Exception e)
            {
                SetStatus($"Join failed: {e.Message}");
                Debug.LogError($"Join error: {e}");
            }
        }
        
        public void StartGame()
        {
            if (!_isHost)
                return;
            
            OnGameStarted?.Invoke();
            
            NetworkManager.Singleton.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        
        public async Task LeaveSession()
        {
            try
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    NetworkManager.Singleton.Shutdown();
                }
                else if (NetworkManager.Singleton.IsClient)
                {
                    NetworkManager.Singleton.Shutdown();
                }
                
                OnDisconnected?.Invoke();
                SetStatus("Disconnected");
            }
            catch (Exception e)
            {
                Debug.LogError($"Leave error: {e}");
            }
        }
        
        public bool IsHost()
        {
            return _isHost;
        }
        
        public string GetJoinCode()
        {
            return _joinCode;
        }
        
        private void OnServerStarted()
        {
            Debug.Log("Server started");
        }
        
        private void OnClientConnected(ulong clientId)
        {
            int count = NetworkManager.Singleton.ConnectedClients.Count;
            OnPlayersCountChanged?.Invoke(count);
            
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log($"Local player connected: {clientId}");
            }
            else
            {
                Debug.Log($"Remote player connected: {clientId}");
            }
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            var count = NetworkManager.Singleton.ConnectedClients.Count;
            
            OnPlayersCountChanged?.Invoke(count);
            
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                OnDisconnected?.Invoke();
            }
        }
        
        private void SetStatus(string message)
        {
            OnStatusChanged?.Invoke(message);
            Debug.Log($"[Status] {message}");
        }
        
        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            }
        }
    }
}
