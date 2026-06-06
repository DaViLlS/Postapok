using _Project.Core.Scripts;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Game.Scripts.UI
{
    public class GameSceneUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text playersCountText;
        [SerializeField] private TMP_Text pingText;
        [SerializeField] private TMP_Text gameStatusText;
        [SerializeField] private Button leaveButton;
        [SerializeField] private GameObject pauseMenu;
        
        private MultiplayerManager multiplayerManager;
        private GameManager gameManager;
        private bool isPaused = false;
        
        private void Start()
        {
            // Находим менеджеры
            multiplayerManager = FindObjectOfType<MultiplayerManager>();
            gameManager = FindObjectOfType<GameManager>();
            
            if (multiplayerManager == null)
                Debug.LogError("MultiplayerManager not found!");
                
            if (gameManager == null)
                Debug.LogError("GameManager not found!");
            
            // Подписываемся на события MultiplayerManager
            if (multiplayerManager != null)
            {
                multiplayerManager.OnStatusChanged += UpdateGameStatus;
                multiplayerManager.OnDisconnected += HandleDisconnect;
            }
            
            // Подписываемся на события GameManager
            if (gameManager != null)
            {
                gameManager.OnPlayersCountChanged += UpdatePlayersCount;
            }
            
            // Кнопка выхода
            if (leaveButton != null)
            {
                leaveButton.onClick.AddListener(LeaveGame);
            }
            
            // Скрываем меню паузы
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            
            // Начальное обновление UI
            UpdatePlayersCount(NetworkManager.Singleton.ConnectedClients.Count);
        }
        
        private void Update()
        {
            HandlePauseInput();
            UpdatePing();
        }
        
        private void HandlePauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        private void TogglePause()
        {
            isPaused = !isPaused;
            
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(isPaused);
            }
            
            Time.timeScale = isPaused ? 0f : 1f;
        }
        
        private void UpdatePing()
        {
            if (pingText != null && NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
            {
                var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
                int ping = (int)(transport.GetCurrentRtt(NetworkManager.Singleton.LocalClientId) * 1000);
                pingText.text = $"Ping: {ping}ms";
            }
        }
        
        private void UpdatePlayersCount(int count)
        {
            if (playersCountText != null)
            {
                playersCountText.text = $"Players: {count}/4";
            }
        }
        
        private void UpdateGameStatus(string status)
        {
            if (gameStatusText != null)
            {
                gameStatusText.text = status;
            }
        }
        
        private void LeaveGame()
        {
            if (multiplayerManager != null)
            {
                multiplayerManager.LeaveSession();
            }
        }
        
        private void HandleDisconnect()
        {
            // Возвращаемся в главное меню при отключении
            Time.timeScale = 1f; // Восстанавливаем время
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        
        private void OnDestroy()
        {
            // Отписываемся от событий
            if (multiplayerManager != null)
            {
                multiplayerManager.OnStatusChanged -= UpdateGameStatus;
                multiplayerManager.OnDisconnected -= HandleDisconnect;
            }
            
            if (gameManager != null)
            {
                gameManager.OnPlayersCountChanged -= UpdatePlayersCount;
            }
            
            Time.timeScale = 1f; // Восстанавливаем время при уничтожении
        }
    }
}