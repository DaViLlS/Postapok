using System.Collections.Generic;
using System.Linq;
using _Project.Main.UiBase;
using _Project.Tutorial;
using _Project.Tutorial.ShortTutorial.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.MainCharacter.Scripts.UI.PotionsInventory
{
    public class PotionsBar : GameScreen
    {
        [Inject] private TutorialDataController _tutorialDataController;
        
        [SerializeField] private ShortGameTutorialConfig shortTutorialGameConfig;
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private PotionElement potionElementPrefab;
        [SerializeField] private Transform[] potionsContainers;

        private List<PotionElement> _potionElements = new List<PotionElement>(8);
        private int _potionIndex = 0;

        public override void Initialize()
        {
            mainCharacter.InputActions.Player.PotionsBar.performed += OpenPotionsBar;
            mainCharacter.InputActions.Player.PotionsBar.canceled += ClosePotionsBar;
        }

        public override void Dispose()
        {
            mainCharacter.InputActions.Player.PotionsBar.performed -= OpenPotionsBar;
            mainCharacter.InputActions.Player.PotionsBar.canceled -= ClosePotionsBar;
        }
        
        private void OpenPotionsBar(InputAction.CallbackContext context)
        {
            Open();
        }
        
        private void ClosePotionsBar(InputAction.CallbackContext context)
        {
            Close();
        }

        /*private void UpdateView()
        {
            List<PotionElement> potionsToRemove = new List<PotionElement>(8);
            
            foreach (var potionElement in _potionElements)
            {
                /*if (!PotionsInventory.ContainsKey(potionElement.Potion))
                {
                    potionsToRemove.Add(potionElement);
                }#1#
            }

            foreach (var potionElement in potionsToRemove)
            {
                _potionElements.Remove(potionElement);
                //potionElement.OnClick -= UsePotion;
                Destroy(potionElement.gameObject);
            }
            
            foreach (var potionKv in PotionsInventory)
            {
                PotionElement potionElement = null;
                
                if (_potionElements.Count == 0)
                {
                    potionElement = Instantiate(potionElementPrefab, potionsContainers[_potionIndex]);
                    /*var itemData = _tradingItemsConfig.Items.First(x => x.itemId == potionKv.Key.PotionData.itemId);
                    potionElement.Setup(potionKv.Key, itemData.icon, potionKv.Value);
                    potionElement.OnClick += UsePotion;#1#
                    _potionElements.Add(potionElement);
                    continue;
                }
                
                potionElement = _potionElements.FirstOrDefault(x => x.Potion.PotionData.potionType == potionKv.Key.PotionData.potionType);

                if (potionElement == null)
                {
                    _potionIndex++;
                    potionElement = Instantiate(potionElementPrefab, potionsContainers[_potionIndex]);
                    /*var itemData = _tradingItemsConfig.Items.First(x => x.itemId == potionKv.Key.PotionData.itemId);
                    potionElement.Setup(potionKv.Key, itemData.icon, potionKv.Value);
                    potionElement.OnClick += UsePotion;#1#
                    _potionElements.Add(potionElement);
                }
                else
                {
                    potionElement.UpdateView(potionKv.Value);
                }
            }
            
            if (!_tutorialDataController.PotionTutorialCompleted)
            {
                mainCharacter.ShortGameTypeTutorial.Setup(shortTutorialGameConfig);
                mainCharacter.ShortGameTypeTutorial.Open();
                _tutorialDataController.PotionTutorialCompleted = true;
            }
        }*/

        /*private void UsePotion(Potion potion)
        {
            mainCharacter.GameInventory.UsePotion(potion.PotionData);
        }*/
    }
}