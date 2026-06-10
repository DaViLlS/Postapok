using UnityEngine;
using UnityEngine.UI;

namespace _Project.Stats.UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image fillableImage;
        [SerializeField] private Image diseaseFillImage;
        [SerializeField] private Color unlockedColor; 
        [SerializeField] private Color lockedColor;
        [SerializeField] private bool isLockedOnStart;
        
        private Health _health;

        public void Initialize(Health health)
        {
            if (isLockedOnStart)
            {
                fillableImage.color = lockedColor;
            }
            
            _health = health;
            fillableImage.fillAmount = health.CurrentHealth / health.MaxHealth;
            
            health.OnHealthChanged += ChangeHealthBarView;
        }

        public void ChangeLockState(bool isLocked)
        {
            if (isLocked)
            {
                fillableImage.color = lockedColor;
            }
            else
            {
                fillableImage.color = unlockedColor;
            }
        }
        
        private void ChangeHealthBarView()
        {
            fillableImage.fillAmount = _health.CurrentHealth / _health.MaxHealth;
            
            if (diseaseFillImage != null)
                diseaseFillImage.fillAmount = _health.CurrentDiseaseLevel / _health.MaxHealth;
        }
    }
}