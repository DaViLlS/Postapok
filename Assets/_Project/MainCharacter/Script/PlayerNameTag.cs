using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace _Project.MainCharacter.Script
{
    public class PlayerNameTag : NetworkBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    
        private void Start()
        {
            if (nameText == null)
            {
                CreateNameTag();
            }
        }
    
        private void CreateNameTag()
        {
            // Создаем Canvas в world space для 2D
            GameObject canvasObj = new GameObject("NameTag");
            canvasObj.transform.SetParent(transform);
            canvasObj.transform.localPosition = offset;
        
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 1;
        
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(2, 1);
            rectTransform.localScale = Vector3.one * 0.01f;
        
            nameText = canvasObj.AddComponent<TextMeshProUGUI>();
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.fontSize = 4;
            nameText.color = Color.white;
            nameText.fontStyle = FontStyles.Bold;
        
            // Добавляем тень для читаемости
            nameText.outlineWidth = 0.2f;
            nameText.outlineColor = Color.black;
        }
    
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        
            if (IsOwner)
            {
                SetPlayerNameServerRpc($"Player {OwnerClientId}");
            }
        }
    
        [ServerRpc]
        private void SetPlayerNameServerRpc(string playerName)
        {
            SetPlayerNameClientRpc(playerName);
        }
    
        [ClientRpc]
        private void SetPlayerNameClientRpc(string playerName)
        {
            if (nameText != null)
                nameText.text = playerName;
        }
    }
}