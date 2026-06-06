using Unity.Netcode;
using UnityEngine;

namespace _Project.MainCharacter.Script
{
    public class PlayerShooting : NetworkBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private int damage = 25;
        
        [Header("Visual")]
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private float muzzleFlashDuration = 0.05f;
        
        private float nextFireTime;
        private Camera mainCamera;
        
        private void Start()
        {
            // Находим камеру при старте
            FindCamera();
            
            if (firePoint == null)
                firePoint = transform.Find("FirePoint");
                
            if (muzzleFlash != null)
                muzzleFlash.SetActive(false);
        }
        
        private void FindCamera()
        {
            // Ищем камеру только если мы владелец
            if (IsOwner)
            {
                mainCamera = Camera.main;
                
                // Если Camera.main не находит (например камера на игроке)
                if (mainCamera == null)
                {
                    mainCamera = FindObjectOfType<Camera>();
                }
            }
        }
        
        private void Update()
        {
            if (!IsOwner) return;
            
            // Проверяем что камера существует
            if (mainCamera == null)
            {
                // Пытаемся найти камеру снова
                FindCamera();
                return; // Пропускаем кадр если камера не найдена
            }
            
            HandleShooting();
        }
        
        private void HandleShooting()
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        
        private void Shoot()
        {
            // Дополнительная проверка камеры
            if (mainCamera == null) return;
            
            try
            {
                // Определяем направление выстрела (к мыши)
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                Vector2 direction = (mousePosition - firePoint.position).normalized;
                
                // Вызываем на сервере
                ShootServerRpc(direction, firePoint.position);
                
                // Локальная визуализация
                ShowMuzzleFlashClientRpc();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Shoot error: {e.Message}");
                // Пробуем переподключить камеру
                FindCamera();
            }
        }
        
        [ServerRpc]
        private void ShootServerRpc(Vector2 direction, Vector3 spawnPosition)
        {
            // Создаем снаряд на сервере
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.GetComponent<NetworkObject>().Spawn();
            
            // Настраиваем снаряд
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(direction, projectileSpeed, damage, OwnerClientId);
            }
        }
        
        [ClientRpc]
        private void ShowMuzzleFlashClientRpc()
        {
            if (muzzleFlash != null)
            {
                StartCoroutine(FlashMuzzle());
            }
        }
        
        private System.Collections.IEnumerator FlashMuzzle()
        {
            if (muzzleFlash == null) yield break;
            
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(muzzleFlashDuration);
            
            if (muzzleFlash != null)
                muzzleFlash.SetActive(false);
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // Переподключаем камеру при спавне
            if (IsOwner)
            {
                FindCamera();
            }
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            // Очищаем ссылку при деспавне
            mainCamera = null;
        }
        
        private void OnEnable()
        {
            // Находим камеру при включении объекта
            if (IsOwner)
            {
                FindCamera();
            }
        }
        
        private void OnDisable()
        {
            // Очищаем ссылку при выключении
            mainCamera = null;
        }
    }
}