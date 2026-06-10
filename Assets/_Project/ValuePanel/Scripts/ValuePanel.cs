using System.Linq;
using _Project.Main.Values.Scripts;
using _Project.MainCharacter.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ValueType = _Project.Main.Values.Scripts.ValueType;

namespace _Project.ValuePanel.Scripts
{
    public class ValuePanel : MonoBehaviour
    {
        [Inject] private ValuesConfiguration _valuesConfiguration;
        [Inject] private MainCharacterData _mainCharacterData;
        
        [SerializeField] private ValueType valueType;
        [Space]
        [SerializeField] private Image valueImage;
        [SerializeField] private TMP_Text value;

        private void Awake()
        {
            var valueData = _valuesConfiguration.Values.First(x => x.valueType == valueType);
            valueImage.sprite = valueData.valueSprite;
            
            if (valueType == ValueType.SoftValue)
            {
                value.text = _mainCharacterData.SoftValue.ToString();
                _mainCharacterData.OnSoftValueChanged += UpdateSoftValueChanged;
            }

            if (valueType == ValueType.HardValue)
            {
                value.text = _mainCharacterData.HardValue.ToString();
                _mainCharacterData.OnHardValueChanged += UpdateHardValueChanged;
            }
        }

        private void OnDestroy()
        {
            if (valueType == ValueType.SoftValue)
                _mainCharacterData.OnSoftValueChanged -= UpdateSoftValueChanged;
            
            if (valueType == ValueType.HardValue)
                _mainCharacterData.OnHardValueChanged -= UpdateHardValueChanged;
        }

        private void UpdateSoftValueChanged()
        {
            value.text = _mainCharacterData.SoftValue.ToString();
        }
        
        private void UpdateHardValueChanged()
        {
            value.text = _mainCharacterData.HardValue.ToString();
        }
    }
}