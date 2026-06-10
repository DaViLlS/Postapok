using _Project.Main.UiBase;
using UnityEngine.SceneManagement;

namespace _Project.Tutorial.Scripts.UI
{
    public class TutorialResultScreen : GameScreen
    {
        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public void ShowResultScreen()
        {
            gameObject.SetActive(true);
        }
        
        public void ExitLevel()
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("GameStarter");
        }
    }
}