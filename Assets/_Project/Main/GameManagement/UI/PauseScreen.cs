using _Project.Main.NavigationUI;
using _Project.Main.UiBase;
using _Project.MainCharacter.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Main.GameManagement.UI
{
    public class PauseScreen : GameScreen
    {
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private GameObject background;
        [SerializeField] private NavigationPanel navigationPanel;

        private bool _isActive;
        
        public override void Initialize()
        {
            _isActive = false;
            navigationPanel.Initialize();
        }

        public override void Dispose()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (navigationPanel.IsAnimating)
                    return;
                
                if (_isActive)
                {
                    mainCharacter.Movement.LockMovement();
                    mainCharacter.Movement.LockCameraStateChanging();
                    _isActive = false;
                    navigationPanel.Hide();
                    background.SetActive(false);
                }
                else
                {
                    mainCharacter.Movement.UnlockMovement();
                    mainCharacter.Movement.UnlockCameraStateChanging();
                    _isActive = true;
                    background.SetActive(true);
                    navigationPanel.Show();
                }
            }
        }

        public void Continue()
        {
            _isActive = false;
            navigationPanel.Hide();
            background.SetActive(false);
        }

        public void ExitLevel()
        {
            background.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("GameStarter");
            }
        }
    }
}