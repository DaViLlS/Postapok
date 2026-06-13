using _Project.Stats.UI;
using UnityEngine;
using Zenject;

namespace _Project.MainCharacter.Scripts.UI
{
    public class MainCharacterStatsView : MonoBehaviour
    {
        [Inject] private MainCharacterController _mainCharacter;
        
        [SerializeField] private HealthView healthView;
        [SerializeField] private StaminaView staminaView;

        private void Start()
        {
            return;
            
            healthView.Initialize(_mainCharacter.Health);
            staminaView.Initialize(_mainCharacter.Stamina);
        }
    }
}