using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Core.Scripts
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject lobbyPanel;
        
        [Header("Main Menu")]
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Button hostButton;
        [SerializeField] private TMP_InputField joinCodeInput;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button quitButton;
        
        [Header("Lobby")]
        [SerializeField] private TMP_Text lobbyCodeText;
        [SerializeField] private TMP_Text lobbyPlayersText;
        [SerializeField] private Button copyCodeButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button leaveButton;
        
        private MultiplayerManager multiplayerManager;
        
        private void Start()
        {
            // Находим MultiplayerManager (он должен быть в DontDestroyOnLoad)
            multiplayerManager = FindObjectOfType<MultiplayerManager>();
            
            if (multiplayerManager == null)
            {
                Debug.LogError("MultiplayerManager not found!");
                return;
            }
            
            // Подписываемся на события
            multiplayerManager.OnStatusChanged += UpdateStatus;
            multiplayerManager.OnLobbyCreated += ShowLobby;
            multiplayerManager.OnPlayersCountChanged += UpdatePlayersCount;
            multiplayerManager.OnDisconnected += ShowMainMenu;
            
            // Кнопки
            hostButton.onClick.AddListener(() => multiplayerManager.StartHost());
            joinButton.onClick.AddListener(() => multiplayerManager.JoinGame(joinCodeInput.text.Trim()));
            quitButton.onClick.AddListener(QuitGame);
            
            copyCodeButton.onClick.AddListener(CopyCodeToClipboard);
            startGameButton.onClick.AddListener(() => multiplayerManager.StartGame());
            leaveButton.onClick.AddListener(() => multiplayerManager.LeaveSession());
            
            ShowMainMenu();
        }
        
        public void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            SetButtonsInteractable(true);
        }
        
        private void ShowLobby(string joinCode)
        {
            mainMenuPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            lobbyCodeText.text = $"Code: {joinCode}";
            
            // Кнопка Start только для хоста
            startGameButton.gameObject.SetActive(multiplayerManager.IsHost());
        }
        
        private void UpdateStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
        
        private void UpdatePlayersCount(int count)
        {
            if (lobbyPlayersText != null)
                lobbyPlayersText.text = $"Players: {count}";
        }
        
        private void SetButtonsInteractable(bool interactable)
        {
            hostButton.interactable = interactable;
            joinButton.interactable = interactable;
            joinCodeInput.interactable = interactable;
        }
        
        private void CopyCodeToClipboard()
        {
            string code = lobbyCodeText.text.Replace("Code: ", "");
            GUIUtility.systemCopyBuffer = code;
            
            copyCodeButton.GetComponentInChildren<TMP_Text>().text = "Copied!";
            Invoke(nameof(ResetCopyButtonText), 2f);
        }
        
        private void ResetCopyButtonText()
        {
            copyCodeButton.GetComponentInChildren<TMP_Text>().text = "Copy Code";
        }
        
        private void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        private void OnDestroy()
        {
            if (multiplayerManager != null)
            {
                multiplayerManager.OnStatusChanged -= UpdateStatus;
                multiplayerManager.OnLobbyCreated -= ShowLobby;
                multiplayerManager.OnPlayersCountChanged -= UpdatePlayersCount;
                multiplayerManager.OnDisconnected -= ShowMainMenu;
            }
        }
    }
}