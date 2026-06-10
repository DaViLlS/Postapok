using _Project.Main.GameManagement;
using _Project.Tutorial.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Tutorial.Scripts
{
    public class TutorialGame : GameLevelController
    {
        [SerializeField] private TutorialController tutorialController;
        [SerializeField] private TutorialResultScreen tutorialResultScreen;
        [SerializeField] private EntrancePlot entrancePlot;
        [SerializeField] private EntrancePlot endingEntrancePlot;

        protected override void Awake()
        {
            base.Awake();

            entrancePlot.OnEntranceEnded += StartTutorial;
            tutorialController.OnTutorialCompleted += CheckEndGameAvailability;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            entrancePlot.OnEntranceEnded -= StartTutorial;
            tutorialController.OnTutorialCompleted -= CheckEndGameAvailability;
            endingEntrancePlot.OnEntranceEnded -= ChangeScene;
        }
        
        private void StartTutorial()
        {
            entrancePlot.gameObject.SetActive(false);
            entrancePlot.OnEntranceEnded -= StartTutorial;
            tutorialController.StartTutorial();
        }

        protected override void CheckEndGameAvailability()
        {
            endingEntrancePlot.OnEntranceEnded += ChangeScene;
            endingEntrancePlot.Show();
        }

        private void ChangeScene()
        {
            endingEntrancePlot.OnEntranceEnded -= ChangeScene;
            SceneManager.LoadScene("MeetWithDeath");
        }

        public override string GetLevelTaskText()
        {
            return GetTaskTextMask();
        }

        protected override void CompleteLevel()
        {
            
        }

        protected override void FailLevel()
        {
            
        }
    }
}