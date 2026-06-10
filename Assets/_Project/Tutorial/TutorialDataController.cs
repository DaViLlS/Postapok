namespace _Project.Tutorial
{
    public class TutorialDataController
    {
        public bool FirstTutorialCompleted;
        public bool MainMenuTutorialCompleted;
        public bool RescueTutorialCompleted;
        public bool TotemsTutorialCompleted;
        public bool PotionTutorialCompleted;
        public bool TraderTutorialCompleted;

        public void Setup()
        {
            FirstTutorialCompleted = false;
            MainMenuTutorialCompleted = false;
        }

        public void Setup(TutorialSaveData saveData)
        {
            FirstTutorialCompleted = saveData.firstTutorialCompleted;
            MainMenuTutorialCompleted = saveData.mainMenuTutorialCompleted;
            RescueTutorialCompleted = saveData.rescueTutorialCompleted;
            TotemsTutorialCompleted = saveData.totemsTutorialCompleted;
            PotionTutorialCompleted = saveData.potionTutorialCompleted;
            TraderTutorialCompleted = saveData.traderTutorialCompleted;
        }
    }
}