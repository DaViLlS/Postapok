using Unity.Netcode;
using UnityEngine;

namespace _Project.Shooting
{
    public class Projectile : NetworkBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private LayerMask collisionMask;
        
        private Vector2 moveDirection;
        private float moveSpeed;
        private int damage;
        private ulong ownerId;
        private float spawnTime;
        
        public void Initialize(Vector2 direction, float speed, int dmg, ulong owner)
        {
            moveDirection = direction;
            moveSpeed = speed;
            damage = dmg;
            ownerId = owner;
            spawnTime = Time.time;
            
            // Поворачиваем снаряд в направлении движения
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        private void Update()
        {
            if (!IsServer) return;
            
            // Движение снаряда
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
            
            // Уничтожение по времени
            if (Time.time - spawnTime >= lifetime)
            {
                DestroyProjectile();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer)
                return;
            
            // Проверяем, что не попали в себя
            NetworkObject hitNetworkObject = other.GetComponent<NetworkObject>();
            if (hitNetworkObject != null && hitNetworkObject.OwnerClientId == ownerId)
                return;
            
            // Наносим урон
            /*HealthSystem.HealthSystem health = other.GetComponent<HealthSystem.HealthSystem>();
            
            if (health != null)
            {
                health.TakeDamageServerRpc(damage);
            }*/
            
            // Эффект попадания
            SpawnHitEffectClientRpc(transform.position);
            
            // Уничтожаем снаряд
            DestroyProjectile();
        }
        
        [ClientRpc]
        private void SpawnHitEffectClientRpc(Vector3 position)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, position, Quaternion.identity);
            }
        }
        
        private void DestroyProjectile()
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}