using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Localization;
using _Project.Main.UiBase;
using _Project.MainCharacter.Scripts;
using _Project.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.TasksAndDialogues.Tasks.Scripts.UI
{
    public class TasksWindow : GameScreen
    {
        [Inject] private TasksController _tasksController;
        [Inject] private RewardsIconConfig _rewardsIconConfig;
        [Inject] private IInstantiator _instantiator;
        [Inject] private LocalizationController _localizationController;
        [Inject] private MainCharacterData _mainCharacterData;

        [SerializeField] private Transform firstTasksContainer;
        [SerializeField] private Transform secondTasksContainer;
        [SerializeField] private TaskElement taskElementPrefab;
        [Space] 
        [SerializeField] private TMP_Text dailyTasksResetTime;
        [SerializeField] private TMP_Text resetCountText;
        [SerializeField] private ulong resetCount;
        
        private List<TaskElement> _mainPlotTasks = new List<TaskElement>();
        private List<TaskElement> _secondaryPlotTasks = new List<TaskElement>();
        private List<TaskElement> _dailyTasks = new List<TaskElement>();

        private float updateInterval = 1f;
        private float nextUpdateTime = 0f;

        private bool _isDailyTasksActive;
        
        public override void Initialize()
        {
            SetupMainPlotTasksView();
            SetupSecondaryPlotTasksView();
            SetupDailyTasksView();
            
            resetCountText.text = resetCount.ToString();
            
            _tasksController.OnTaskUpdated += UpdateView;
            _tasksController.OnTaskCompleted += UpdateView;
            _localizationController.OnLanguageChanged += UpdateView;
        }

        public override void Dispose()
        {
            _tasksController.OnTaskUpdated -= UpdateView;
            _tasksController.OnTaskCompleted -= UpdateView;
            _localizationController.OnLanguageChanged -= UpdateView;
            _tasksController.OnTasksReset -= UpdateDailyTasks;

            foreach (var dailyTask in _dailyTasks)
            {
                dailyTask.OnRewardClaimed -= RemoveTaskElement;
            }
        }

        public void ChangeToMainPlotView()
        {
            _isDailyTasksActive = false;
            
            foreach (var taskElement in _dailyTasks)
            {
                dailyTasksResetTime.gameObject.SetActive(false);
                taskElement.gameObject.SetActive(false);
            }
            
            foreach (var taskElement in _secondaryPlotTasks)
            {
                taskElement.gameObject.SetActive(true);
            }

            foreach (var taskElement in _mainPlotTasks)
            {
                taskElement.gameObject.SetActive(true);
            }
        }

        public void ChangeToDailyTasksView()
        {
            _isDailyTasksActive = true;
            
            foreach (var taskElement in _secondaryPlotTasks)
            {
                taskElement.gameObject.SetActive(false);
            }
            
            foreach (var taskElement in _mainPlotTasks)
            {
                taskElement.gameObject.SetActive(false);
            }

            foreach (var taskElement in _dailyTasks)
            {
                dailyTasksResetTime.gameObject.SetActive(true);
                taskElement.gameObject.SetActive(true);
            }
        }

        public void ResetDailyTasks()
        {
            if (_mainCharacterData.HardValue >= resetCount)
            {
                _tasksController.ResetQuests();
                _mainCharacterData.RemoveHardValue(resetCount);
            }
        }

        private void SetupMainPlotTasksView()
        {
            foreach (var task in _tasksController.CurrentMainPlotTasks)
            {
                if (task.IsCompleted && task.IsRewardClaimed)
                    continue;
                
                var taskElement = _instantiator.InstantiatePrefabForComponent<TaskElement>(taskElementPrefab, firstTasksContainer);

                if (task.RewardData == null)
                {
                    taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType);
                }
                else
                {
                    taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType, task.RewardData,
                        _rewardsIconConfig.GetIconByType(task.RewardData.rewardType));
                }
                
                taskElement.GetFillImage().gameObject.SetActive(false);
                taskElement.OnRewardClaimed += RemoveTaskElement;
                _mainPlotTasks.Add(taskElement);
            }
        }

        private void SetupSecondaryPlotTasksView()
        {
            foreach (var task in _tasksController.CurrentSecondaryPlotTasks)
            {
                if (task.IsCompleted && task.IsRewardClaimed)
                    continue;
                
                var taskElement = _instantiator.InstantiatePrefabForComponent<TaskElement>(taskElementPrefab, secondTasksContainer);
                
                if (task.RewardData == null)
                {
                    taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType);
                }
                else
                {
                    taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType, task.RewardData,
                        _rewardsIconConfig.GetIconByType(task.RewardData.rewardType));
                }
                
                taskElement.GetFillImage().gameObject.SetActive(false);
                taskElement.OnRewardClaimed += RemoveTaskElement;
                _secondaryPlotTasks.Add(taskElement);
            }
        }

        private void SetupDailyTasksView()
        {
            _tasksController.OnTasksReset += UpdateDailyTasks;

            dailyTasksResetTime.gameObject.SetActive(false);
            
            foreach (var task in _tasksController.CurrentDailyTasks)
            {
                if (task.IsCompleted && task.IsRewardClaimed)
                    continue;
                
                var taskElement = _instantiator.InstantiatePrefabForComponent<TaskElement>(taskElementPrefab, firstTasksContainer);
                taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType, task.RewardData,
                    _rewardsIconConfig.GetIconByType(task.RewardData.rewardType));
                task.FillTaskViewImage(taskElement.GetFillImage());
                taskElement.OnRewardClaimed += RemoveTaskElement;
                _dailyTasks.Add(taskElement);
                taskElement.gameObject.SetActive(_isDailyTasksActive);
            }
        }

        private void UpdateDailyTasks()
        {
            nextUpdateTime = 0f;
            
            foreach (var task in _dailyTasks)
            {
                task.OnRewardClaimed -= RemoveTaskElement;
                Destroy(task.gameObject);
            }
            
            _dailyTasks.Clear();
            
            foreach (var task in _tasksController.CurrentDailyTasks)
            {
                if (task.IsCompleted && task.IsRewardClaimed)
                    continue;
                
                var taskElement = _instantiator.InstantiatePrefabForComponent<TaskElement>(taskElementPrefab, firstTasksContainer);
                taskElement.Setup(task, task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted, task.TaskType, task.RewardData,
                    _rewardsIconConfig.GetIconByType(task.RewardData.rewardType));
                task.FillTaskViewImage(taskElement.GetFillImage());
                taskElement.OnRewardClaimed += RemoveTaskElement;
                _dailyTasks.Add(taskElement);
                taskElement.gameObject.SetActive(_isDailyTasksActive);
            }
        }
        
        private void LateUpdate()
        {
            if (Time.time >= nextUpdateTime)
            {
                nextUpdateTime = Time.time + updateInterval;
                UpdateTimerDisplay(_tasksController.TimeUntilReset);
            }
        }
    
        private void UpdateTimerDisplay(TimeSpan timeUntilReset)
        {
            if (timeUntilReset.TotalSeconds <= 0)
            {
                if (_localizationController.ChosenLanguage == LanguageType.English)
                    dailyTasksResetTime.text = "0h:00m:00s";
                else if(_localizationController.ChosenLanguage == LanguageType.Russian)
                    dailyTasksResetTime.text = "0ч:00м:00с";
            }
            else
            {
                if (_localizationController.ChosenLanguage == LanguageType.English)
                {
                    dailyTasksResetTime.text = string.Format("{0}h:{1:D2}m:{2:D2}s", 
                        (int)timeUntilReset.TotalHours, 
                        timeUntilReset.Minutes, 
                        timeUntilReset.Seconds);
                }
                else if (_localizationController.ChosenLanguage == LanguageType.Russian)
                {
                    dailyTasksResetTime.text = string.Format("{0}ч:{1:D2}м:{2:D2}с", 
                        (int)timeUntilReset.TotalHours, 
                        timeUntilReset.Minutes, 
                        timeUntilReset.Seconds);
                }
            }
        }

        private void RemoveTaskElement(TaskElement taskElement)
        {
            if (_dailyTasks.Contains(taskElement))
                _dailyTasks.Remove(taskElement);
            
            if (_mainPlotTasks.Contains(taskElement))
                _mainPlotTasks.Remove(taskElement);
            
            if (_secondaryPlotTasks.Contains(taskElement))
                _secondaryPlotTasks.Remove(taskElement);
            
            taskElement.OnRewardClaimed -= RemoveTaskElement;
            Destroy(taskElement.gameObject);
        }

        private void UpdateView()
        {
            foreach (var dailyTask in _dailyTasks)
            {
                var task = _tasksController.CurrentDailyTasks.First(x => x.TaskType == dailyTask.TaskType);
                dailyTask.UpdateView(task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted);
                task.FillTaskViewImage(dailyTask.GetFillImage());
            }
            
            foreach (var mainPlotTask in _mainPlotTasks)
            {
                var task = _tasksController.CurrentMainPlotTasks.First(x => x.TaskType == mainPlotTask.TaskType);
                mainPlotTask.UpdateView(task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted);
                task.FillTaskViewImage(mainPlotTask.GetFillImage());
            }
            
            foreach (var secondaryTask in _secondaryPlotTasks)
            {
                var task = _tasksController.CurrentSecondaryPlotTasks.First(x => x.TaskType == secondaryTask.TaskType);
                secondaryTask.UpdateView(task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted);
                task.FillTaskViewImage(secondaryTask.GetFillImage());
            }
        }

        private void UpdateView(Task task)
        {
            var taskElement = _dailyTasks.FirstOrDefault(x => x.TaskType == task.TaskType);
                
            if (taskElement != null)
            {
                taskElement.UpdateView(task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted);
                task.FillTaskViewImage(taskElement.GetFillImage());
            }
            else
            {
                taskElement = _mainPlotTasks.FirstOrDefault(x => x.TaskType == task.TaskType);

                if (taskElement == null)
                {
                    taskElement = _secondaryPlotTasks.FirstOrDefault(x => x.TaskType == task.TaskType);
                }
                
                if (taskElement != null)
                {
                    taskElement.UpdateView(task.GetTitle(_localizationController.ChosenLanguage), task.IsCompleted);
                    task.FillTaskViewImage(taskElement.GetFillImage());
                }
            }
        }
    }
}