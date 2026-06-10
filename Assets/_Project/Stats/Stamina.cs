using System;
using UnityEngine;

namespace _Project.Stats
{
    public class Stamina
    {
        public event Action OnStaminaChanged;
        
        public float MaxStamina { get; private set; }
        public float CurrentStamina { get; private set; }
        public float LastStamineDecTime { get; private set; }

        public Stamina(float maxStamina)
        {
            MaxStamina = maxStamina;
            CurrentStamina = MaxStamina;
        }

        public void IncreaseStamina(float stamina)
        {
            CurrentStamina += stamina;
            OnStaminaChanged?.Invoke();
        }

        public void DecreaseStamina(float stamina)
        {
            LastStamineDecTime = Time.time;
            CurrentStamina -= stamina;
            OnStaminaChanged?.Invoke();
        }
    }
}