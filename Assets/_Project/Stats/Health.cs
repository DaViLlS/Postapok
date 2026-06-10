using System;
using UnityEngine;

namespace _Project.Stats
{
    public class Health
    {
        public event Action OnZeroHealth;
        public event Action OnHealthChanged;
        
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float LastHealthDecTime { get; private set; }
        public float LastDiseaseTime { get; private set; }
        public float CurrentDiseaseLevel { get; private set; }
        
        public Health(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
        }

        public void Damage(float damage)
        {
            LastHealthDecTime = Time.time;
            
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                OnZeroHealth?.Invoke();
            }
            else
            {
                OnHealthChanged?.Invoke();
            }
        }

        public void MakeDisease(float disease)
        {
            CurrentDiseaseLevel += disease;

            LastDiseaseTime = Time.time;

            if (CurrentHealth > MaxHealth - CurrentDiseaseLevel)
            {
                CurrentHealth = MaxHealth - CurrentDiseaseLevel;
            }
            
            OnHealthChanged?.Invoke();
        }

        public void HealDisease(float heal)
        {
            CurrentDiseaseLevel -= heal;
            
            OnHealthChanged?.Invoke();
        }

        public void Heal(float heal)
        {
            if (Mathf.Approximately(CurrentHealth, MaxHealth))
                return;
            
            CurrentHealth += heal;
            
            OnHealthChanged?.Invoke();
        }
    }
}