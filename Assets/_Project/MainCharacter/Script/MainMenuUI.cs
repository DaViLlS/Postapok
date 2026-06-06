using _Project.Core.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.MainCharacter.Script
{
    public class MainMenuUI : MonoBehaviour
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
            multiplayerManager = FindObjectOfType<MultiplayerManager>();
            
            if (multiplayerManager == null)
            {
                Debug.LogError("MultiplayerManager not found in scene!");
                return;
            }
            
            // Привязка кнопок
            hostButton.onClick.AddListener(() => multiplayerManager.StartHost());
            joinButton.onClick.AddListener(() => multiplayerManager.JoinGame(joinCodeInput.text));
            quitButton.onClick.AddListener(QuitGame);
            
            copyCodeButton.onClick.AddListener(CopyCodeToClipboard);
            startGameButton.onClick.AddListener(() => multiplayerManager.StartGame()); // Теперь public
            leaveButton.onClick.AddListener(() => multiplayerManager.LeaveSession());
            
            ShowMainMenu();
        }
        
        private void Update()
        {
            // Обновляем количество игроков каждый кадр
            if (lobbyPanel.activeSelf && multiplayerManager != null)
            {
                UpdatePlayersCount();
            }
        }
        
        public void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }
        
        public void ShowLobby(string joinCode)
        {
            mainMenuPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            lobbyCodeText.text = $"Code: {joinCode}";
            startGameButton.gameObject.SetActive(false); // Изначально скрыта, пока хост не подключится
        }
        
        public void ShowStartButton()
        {
            // Показываем кнопку Start только хосту
            startGameButton.gameObject.SetActive(true);
        }
        
        public void UpdateStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
        
        public void UpdatePlayersCount()
        {
            if (lobbyPlayersText != null && Unity.Netcode.NetworkManager.Singleton != null)
            {
                int count = Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count;
                lobbyPlayersText.text = $"Players: {count}";
            }
        }
        
        public void SetButtonsInteractable(bool interactable)
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
            // Отписываемся от событий
            if (hostButton != null)
                hostButton.onClick.RemoveAllListeners();
            if (joinButton != null)
                joinButton.onClick.RemoveAllListeners();
            if (quitButton != null)
                quitButton.onClick.RemoveAllListeners();
            if (copyCodeButton != null)
                copyCodeButton.onClick.RemoveAllListeners();
            if (startGameButton != null)
                startGameButton.onClick.RemoveAllListeners();
            if (leaveButton != null)
                leaveButton.onClick.RemoveAllListeners();
        }
    }
}