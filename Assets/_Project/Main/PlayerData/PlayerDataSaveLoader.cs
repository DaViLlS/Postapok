using System.Collections.Generic;
using System.Globalization;
using _Project.Localization;
using _Project.Main.PlayerData.TasksData;
using _Project.Main.SavingSystem;
using _Project.MainCharacter.Scripts;
using _Project.TasksAndDialogues.Tasks.Scripts;
using _Project.Tutorial;
using Zenject;

namespace _Project.Main.PlayerData
{
    public class PlayerDataSaveLoader : ISaveLoader
    {
        [Inject] private LocalizationController _localizationController;
        [Inject] private TutorialDataController _tutorialDataController;
        [Inject] private SoundsController _soundsController;
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private TradingItems _tradingItems;
        [Inject] private TasksController _tasksController;
        
        public void LoadData()
        {
            _localizationController.Setup();

            if (Repository.TryGetData<SoundsSaveData>(out var soundsSaveData))
            {
                _soundsController.Setup(soundsSaveData);
            }
            else
            {
                _soundsController.Setup();
            }

            if (Repository.TryGetData<TutorialSaveData>(out var tutorialSaveData))
            {
                _tutorialDataController.Setup(tutorialSaveData);
            }
            else
            {
                _tutorialDataController.Setup();
            }
            
            //PLAYER SAVE DATA
            if (Repository.TryGetData<PlayerSaveData>(out var playerData))
            {
                _mainCharacterData.Load(playerData);
            }
            else
            {
                _mainCharacterData.Initialize();
            }
            
            //TRADING ITEMS
            if (Repository.TryGetData<TradingItemsSaveData>(out var tradingItemsData))
            {
                _tradingItems.Load(tradingItemsData);
            }
            
            //PLOT TASKS
            if (Repository.TryGetData<PlotTasksSaveData>(out var plotTasksData))
            {
                _tasksController.LoadPlotTasks(plotTasksData);
            }
            
            //DAILY TASKS
            if (Repository.TryGetData<DailyTasksSaveData>(out var dailyTasksSaveData))
            {
                if (dailyTasksSaveData.tasksSavesData.Count == 0)
                {
                    _tasksController.CreateNewDailyTasks();
                }
                else
                {
                    _tasksController.LoadTasks(dailyTasksSaveData);
                }
            }
            else
            {
                _tasksController.CreateNewDailyTasks();
            }
        }

        public void SaveData()
        {
            SaveLocalizationData();
            SaveTutorialData();
            SaveSoundsData();
            SavePlayerMainData();
            SaveTradingItems();
            SaveQuestsData();
        }

        private void SaveLocalizationData()
        {
            var data = new LocalizationSaveData();
            data.chosenLanguage = _localizationController.ChosenLanguage;
            
            Repository.SetData(data);
        }

        private void SaveTutorialData()
        {
            var data = new TutorialSaveData();
            data.firstTutorialCompleted = _tutorialDataController.FirstTutorialCompleted;
            data.mainMenuTutorialCompleted = _tutorialDataController.MainMenuTutorialCompleted;
            data.rescueTutorialCompleted = _tutorialDataController.RescueTutorialCompleted;
            data.totemsTutorialCompleted = _tutorialDataController.TotemsTutorialCompleted;
            data.potionTutorialCompleted = _tutorialDataController.PotionTutorialCompleted;
            data.traderTutorialCompleted = _tutorialDataController.TraderTutorialCompleted;
            
            Repository.SetData(data);
        }

        private void SaveSoundsData()
        {
            var data = new SoundsSaveData();
            data.musicVolume = _soundsController.MusicVolume;
            data.effectsVolume = _soundsController.EffectsVolume;
            data.footstepsVolume = _soundsController.FootstepsVolume;
            
            
            Repository.SetData(data);
        }

        private void SavePlayerMainData()
        {
            var data = new PlayerSaveData();
            data.Setup(_mainCharacterData);
            
            Repository.SetData(data);
        }

        private void SaveTradingItems()
        {
            var data = new TradingItemsSaveData();
            var tradingItemsSaveData = new List<TradingItemSaveData>();

            foreach (var tradingItem in _tradingItems.TradingItemsList)
            {
                var tradingItemSaveData = new TradingItemSaveData();
                tradingItemSaveData.itemId = tradingItem.ItemId;
                tradingItemSaveData.count = tradingItem.Count;
                tradingItemsSaveData.Add(tradingItemSaveData);
            }
            
            data.tradingItems = tradingItemsSaveData;
            
            Repository.SetData(data);
        }

        private void SaveQuestsData()
        {
            var data = new DailyTasksSaveData();
            var tasksSaveData = new List<TaskSaveData>();

            foreach (var task in _tasksController.CurrentDailyTasks)
            {
                var taskSaveData = new TaskSaveData();
                taskSaveData.Setup(task);
                tasksSaveData.Add(taskSaveData);
            }

            data.resetTimeData = _tasksController.NextResetTime.ToString(CultureInfo.InvariantCulture);
            data.tasksSavesData = tasksSaveData;
            
            Repository.SetData(data);

            var plotData = new PlotTasksSaveData();
            var mainTasksSaveData = new List<TaskSaveData>();
            var secondaryTasksSaveData = new List<TaskSaveData>();

            foreach (var task in _tasksController.CurrentMainPlotTasks)
            {
                var taskSaveData = new TaskSaveData();
                taskSaveData.Setup(task);
                mainTasksSaveData.Add(taskSaveData);
            }

            foreach (var task in _tasksController.CurrentSecondaryPlotTasks)
            {
                var taskSaveData = new TaskSaveData();
                taskSaveData.Setup(task);
                secondaryTasksSaveData.Add(taskSaveData);
            }
            
            plotData.mainTasksSaveData = mainTasksSaveData;
            plotData.secondaryTasksSaveData = secondaryTasksSaveData;
            
            Repository.SetData(plotData);
        }
    }
}