using _Project.Main.UiBase;
using UnityEngine;

namespace _Project.MainCharacter.Scripts.UI
{
    public class CharacterWindow : GameScreen
    {
        [SerializeField] private ParametersPanel parametersPanel;
        
        public override void Initialize()
        {
            parametersPanel.Initialize();
        }

        public override void Dispose()
        {
            parametersPanel.Dispose();
        }
    }
}