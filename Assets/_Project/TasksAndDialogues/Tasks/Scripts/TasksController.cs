using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Project.Main.PlayerData.TasksData;
using _Project.MainCharacter.Scripts;
using _Project.Rewards;
using _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Data;
using _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Tasks;
using _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.TasksAndDialogues.Tasks.Scripts
{
    public class TasksController
    {
        public event Action<Task> OnTaskUpdated;
        public event Action<Task> OnTaskCompleted;
        public event Action<TimeSpan> OnTimerUpdated;
        public event Action OnTasksReset;

        [Inject] private MainCharacterData _characterData;
        [Inject] private DailyTasksConfig _dailyTasksConfig;
        [Inject] private MainTasksConfig _mainTasksConfig;
        [Inject] private RewardsController _rewardsController;

        private DateTime _nextResetTime;
        private System.Timers.Timer _updateTimer;
        private SynchronizationContext unityContext;
        
        private List<PlotTask> _currentMainPlotTasks = new List<PlotTask>();
        private List<PlotTask> _currentSecondaryPlotTasks = new List<PlotTask>();
        private List<Task> _currentDailyTasks = new List<Task>();

        public TimeSpan TimeUntilReset => _nextResetTime - DateTime.Now;
        public DateTime NextResetTime => _nextResetTime;
        public List<PlotTask> CurrentMainPlotTasks => _currentMainPlotTasks;
        public List<PlotTask> CurrentSecondaryPlotTasks => _currentSecondaryPlotTasks;
        public List<Task> CurrentDailyTasks => _currentDailyTasks;
        
        public string GetFormattedTimeUntilReset()
        {
            TimeSpan time = TimeUntilReset;
    
            if (time.TotalSeconds <= 0)
                return "0h:00m:00s";
    
            return $"{(int)time.TotalHours}h:{time.Minutes:D2}m:{time.Seconds:D2}s";
        }
        
        public void CreateNewDailyTasks()
        {
            unityContext = SynchronizationContext.Current;
            
            _nextResetTime = DateTime.Now.AddHours(24);

            GenerateQuests();
            StartTimer();
        }

        public void LoadTasks(DailyTasksSaveData dailyTasksSaveData)
        {
            unityContext = SynchronizationContext.Current;
            
            _nextResetTime = DateTime.Parse(dailyTasksSaveData.resetTimeData);

            if (CheckForReset())
            {
                return;
            }
            
            foreach (var taskSaveData in dailyTasksSaveData.tasksSavesData)
            {
                Task task = null;
                
                switch (taskSaveData.taskType)
                {
                    case TaskType.ReachMoney:
                        task = new ReachMoneyValueTask(_dailyTasksConfig.GetUlongTaskByType(taskSaveData.taskType),
                            _characterData, taskSaveData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.DestroyEnemies:
                        task = new DestroyEnemiesTask(_dailyTasksConfig.GetUlongTaskByType(taskSaveData.taskType),
                            _characterData,taskSaveData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.DestroyTotems:
                        task = new DestroyTotemsTask(_dailyTasksConfig.GetUlongTaskByType(taskSaveData.taskType),
                            _characterData, taskSaveData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.ResqueVillagers:
                        task = new ResqueVillagersTask(_dailyTasksConfig.GetUlongTaskByType(taskSaveData.taskType),
                            _characterData, taskSaveData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                }
            }
        }

        private bool CheckForReset()
        {
            if (DateTime.Now >= _nextResetTime)
            {
                ResetQuests();
                OnTasksReset?.Invoke();
                return true;
            }
            else
            {
                // Запускаем таймер до следующего сброса
                StartTimer();
                return false;
            }
        }

        private void StartTimer()
        {
            StopTimer();
        
            // Создаем таймер, который срабатывает каждую секунду
            _updateTimer = new System.Timers.Timer(1000);
            _updateTimer.Elapsed += OnTimerElapsed;
            _updateTimer.AutoReset = true;
            _updateTimer.Start();
        }
        
        private void StopTimer()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer.Elapsed -= OnTimerElapsed;
                _updateTimer.Dispose();
                _updateTimer = null;
            }
        }
    
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            unityContext.Post(_ => 
            {
                TimeSpan timeUntilReset = TimeUntilReset;
            
                if (timeUntilReset.TotalSeconds <= 0)
                {
                    ResetQuests();
                }
                else
                {
                    OnTimerUpdated?.Invoke(timeUntilReset);
                }
            }, null);
        }

        public void ResetQuests()
        {
            Debug.Log("Сброс ежедневных заданий!");

            _nextResetTime = DateTime.Now.AddHours(24);
            GenerateQuests();
            StartTimer();
            
            OnTasksReset?.Invoke();
        }

        private void GenerateQuests()
        {
            _currentDailyTasks.Clear();
            
            var tempTaskTypes = new List<TaskType>(_dailyTasksConfig.TaskTypes);
            
            for (var i = 0; i < _dailyTasksConfig.MaxDailyTasksCount; i++)
            {
                var randomTaskType = tempTaskTypes[Random.Range(0, tempTaskTypes.Count - 1)];
                tempTaskTypes.Remove(randomTaskType);
                Task task = null;

                switch (randomTaskType)
                {
                    case TaskType.ReachMoney:
                        task = new ReachMoneyValueTask(_dailyTasksConfig.GetUlongTaskByType(randomTaskType), _characterData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.DestroyEnemies:
                        task = new DestroyEnemiesTask(_dailyTasksConfig.GetUlongTaskByType(randomTaskType),
                            _characterData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.DestroyTotems:
                        task = new DestroyTotemsTask(_dailyTasksConfig.GetUlongTaskByType(randomTaskType),
                            _characterData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                    case TaskType.ResqueVillagers:
                        task = new ResqueVillagersTask(_dailyTasksConfig.GetUlongTaskByType(randomTaskType),
                            _characterData);
                        task.OnTaskCompleted += PerformTaskCompleted;
                        task.OnTaskConditionChanged += PerformTaskChanged;
                        _currentDailyTasks.Add(task);
                        break;
                }
            }
        }

        private void PerformTaskChanged(Task task)
        {
            OnTaskUpdated?.Invoke(task);
        }

        public void PerformTaskCompleted(Task task)
        {
            OnTaskCompleted?.Invoke(task);
        }

        public void MakeReward(TaskType taskType, RewardData rewardData)
        {
            _rewardsController.ApplyReward(rewardData.rewardType, rewardData.rewardCount, rewardData.itemId);
        }

        public void LoadPlotTasks(PlotTasksSaveData tasksSaveData)
        {
            foreach (var taskSaveData in tasksSaveData.mainTasksSaveData)
            {
                if (taskSaveData.isCompleted && taskSaveData.isRewardClaimed)
                    continue;

                var taskData = _mainTasksConfig.MainTasks.First(x => x.taskName == taskSaveData.taskName);
                var task = new PlotTask(taskSaveData.taskName, TaskType.PlotTask, taskSaveData.levelToCompleteNames,
                    taskSaveData.rewardData, taskData);
                task.OnTaskCompleted += PerformTaskCompleted;
                _currentMainPlotTasks.Add(task);
            }
            
            foreach (var taskSaveData in tasksSaveData.secondaryTasksSaveData)
            {
                if (taskSaveData.isCompleted && taskSaveData.isRewardClaimed)
                    continue;
                
                var taskData = _mainTasksConfig.MainTasks.First(x => x.taskName == taskSaveData.taskName);
                var task = new PlotTask(taskSaveData.taskName, TaskType.PlotTask, taskSaveData.levelToCompleteNames,
                    taskSaveData.rewardData, taskData);
                task.OnTaskCompleted += PerformTaskCompleted;
                _currentSecondaryPlotTasks.Add(task);
            }
        }

        public void AddMainTask(TaskData taskData)
        {
            var task = new PlotTask(taskData.taskName, TaskType.PlotTask, taskData.rewardData, taskData);
            task.OnTaskCompleted += PerformTaskCompleted;
            _currentMainPlotTasks.Add(task);
        }

        public void AddMainTask(TaskData taskData, string[] levelsName)
        {
            var task = new PlotTask(taskData.taskName, TaskType.CompleteLevelsTask, levelsName, taskData.rewardData, taskData);
            task.OnTaskCompleted += PerformTaskCompleted;
            _currentMainPlotTasks.Add(task);
        }

        public void AddSecondaryTask(TaskData taskData)
        {
            var task = new PlotTask(taskData.taskName, TaskType.PlotTask, taskData.rewardData, taskData);
            task.OnTaskCompleted += PerformTaskCompleted;
            _currentSecondaryPlotTasks.Add(task);
        }

        public bool CheckCompleteLevelsTasks(string levelName)
        {
            foreach (var task in _currentMainPlotTasks)
            {
                if (task.IsCompleted)
                    continue;
                
                if (task.LevelToCompleteNames != null)
                {
                    if (task.LevelToCompleteNames.Contains(levelName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void CompleteLevelsTasks(string levelName)
        {
            foreach (var task in _currentMainPlotTasks)
            {
                if (task.IsCompleted)
                    continue;
                
                if (task.LevelToCompleteNames != null)
                {
                    if (task.LevelToCompleteNames.Contains(levelName))
                    {
                        task.CompleteTask();

                        return;
                    }
                }
            }
        }
    }
}