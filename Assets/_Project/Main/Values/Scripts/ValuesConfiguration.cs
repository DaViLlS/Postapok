using System.Collections.Generic;
using UnityEngine;

namespace _Project.Main.Values.Scripts
{
    [CreateAssetMenu(fileName = "ValuesConfiguration", menuName = "Values Configuration")]
    public class ValuesConfiguration : ScriptableObject
    {
        [SerializeField] private List<ValueData> values;
        
        public List<ValueData> Values => values;
    }
}