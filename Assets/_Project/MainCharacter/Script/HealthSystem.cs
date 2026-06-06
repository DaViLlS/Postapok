using System;
using _Project.Game.Scripts;
using Unity.Netcode;
using UnityEngine;

namespace _Project.MainCharacter.Script
{
    public class HealthSystem : NetworkBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool isInvincible = false;
        [SerializeField] private float invincibilityTime = 1f;
        
        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;
        
        [Header("UI")]
        [SerializeField] private UnityEngine.UI.Slider healthBar;
        
        private NetworkVariable<int> currentHealth = new NetworkVariable<int>();
        private float lastDamageTime;
        
        public event Action<int, int> OnHealthChanged; // (current, max)
        public event Action OnDeath;
        
        private void Start()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsServer)
            {
                currentHealth.Value = maxHealth;
            }
            
            currentHealth.OnValueChanged += OnHealthValueChanged;
            
            // Обновляем UI при спавне
            UpdateHealthUI(currentHealth.Value, maxHealth);
        }
        
        private void OnHealthValueChanged(int oldValue, int newValue)
        {
            UpdateHealthUI(newValue, maxHealth);
            OnHealthChanged?.Invoke(newValue, maxHealth);
            
            if (newValue <= 0)
            {
                OnDeath?.Invoke();
                HandleDeath();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(int damage)
        {
            if (isInvincible) return;
            if (Time.time - lastDamageTime < invincibilityTime) return;
            
            currentHealth.Value = Mathf.Max(0, currentHealth.Value - damage);
            lastDamageTime = Time.time;
            
            // Визуальная обратная связь для всех клиентов
            DamageFeedbackClientRpc();
            
            if (currentHealth.Value <= 0)
            {
                HandleDeathOnServer();
            }
        }
        
        [ClientRpc]
        private void DamageFeedbackClientRpc()
        {
            if (spriteRenderer != null)
            {
                StartCoroutine(FlashDamage());
            }
        }
        
        private System.Collections.IEnumerator FlashDamage()
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
        
        private void HandleDeath()
        {
            // Для локального игрока
            if (IsOwner)
            {
                // Визуальный эффект или UI
                Debug.Log("You died!");
            }
        }
        
        private void HandleDeathOnServer()
        {
            // Respawn или другие действия на сервере
            StartCoroutine(RespawnAfterDelay(3f));
        }
        
        private System.Collections.IEnumerator RespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // Респавн игрока
            currentHealth.Value = maxHealth;
            
            // Найти точку спавна
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                // Перемещение на точку спавна
                Transform spawnPoint = FindSpawnPoint();
                if (spawnPoint != null)
                {
                    transform.position = spawnPoint.position;
                }
            }
            
            RespawnClientRpc();
        }
        
        [ClientRpc]
        private void RespawnClientRpc()
        {
            // Визуальные эффекты респавна для всех клиентов
            Debug.Log($"{name} respawned!");
        }
        
        private Transform FindSpawnPoint()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            if (spawnPoints.Length > 0)
            {
                return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform;
            }
            return null;
        }
        
        private void UpdateHealthUI(int current, int max)
        {
            if (healthBar != null)
            {
                healthBar.maxValue = max;
                healthBar.value = current;
            }
        }
        
        public int GetCurrentHealth() => currentHealth.Value;
        public int GetMaxHealth() => maxHealth;
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            currentHealth.OnValueChanged -= OnHealthValueChanged;
        }
    }
}