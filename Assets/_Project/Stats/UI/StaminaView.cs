using UnityEngine;
using UnityEngine.UI;

namespace _Project.Stats.UI
{
    public class StaminaView : MonoBehaviour
    {
        [SerializeField] private Image fillableImage;
        
        private Stamina _stamina;

        public void Initialize(Stamina stamina)
        {
            _stamina = stamina;
            
            fillableImage.fillAmount = stamina.CurrentStamina / stamina.MaxStamina;
            
            stamina.OnStaminaChanged += ChangeStaminaBarView;
        }
        
        private void ChangeStaminaBarView()
        {
            fillableImage.fillAmount = _stamina.CurrentStamina / _stamina.MaxStamina;
        }
    }
}