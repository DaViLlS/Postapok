using _Project.Tutorial.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.MainMenuTutorial.Scripts.Steps
{
    public class OpenMainCharacterWindow : TutorialStep
    {
        [SerializeField] private Button characterWindowButton;
        [SerializeField] private GameObject arrowSignObject;
        
        public override void StartStep()
        {
            base.StartStep();

            arrowSignObject.SetActive(true);
            characterWindowButton.onClick.AddListener(CharacterWindowOpened);
            characterWindowButton.interactable = true;
        }

        private void CharacterWindowOpened()
        {
            arrowSignObject.SetActive(false);
            characterWindowButton.onClick.RemoveListener(CharacterWindowOpened);
            characterWindowButton.interactable = false;
            CompleteStep();
        }
    }
}