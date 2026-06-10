using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.MainCharacter.Scripts.UI.PotionsInventory
{
    public class PotionElement : MonoBehaviour
    {
        //public event Action<Potion> OnClick;
        
        [SerializeField] private Image potionIcon;
        [SerializeField] private TMP_Text potionsCountText;
        
        //private Potion _potion;
        private int _potionsCount;
        
        //public Potion Potion => _potion;

        /*public void Setup(Potion potion, Sprite potionSprite, int potionsCount)
        {
            _potion = potion;
            _potionsCount = potionsCount;
            
            potionIcon.sprite = potionSprite;
            potionsCountText.text = _potionsCount.ToString();
        }*/

        public void UpdateView(int potionsCount)
        {
            _potionsCount = potionsCount;
            
            potionsCountText.text = _potionsCount.ToString();
        }

        public void Click()
        {
            //OnClick?.Invoke(_potion);
        }
    }
}