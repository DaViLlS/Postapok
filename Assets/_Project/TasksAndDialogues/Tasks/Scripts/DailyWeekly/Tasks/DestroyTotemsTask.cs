using _Project.Localization;
using _Project.Main.PlayerData.TasksData;
using _Project.MainCharacter.Scripts;
using _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Tasks
{
    public class DestroyTotemsTask : Task
    {
        private TaskDataReachValueUlong _taskData;
        
        public DestroyTotemsTask(TaskDataReachValueUlong taskData, MainCharacterData characterData)
        {
            TaskType = taskData.taskType;
            var randomRewardData = taskData.randomRewards[Random.Range(0, taskData.randomRewards.Count - 1)];
            RewardData = randomRewardData;
            _taskData = taskData;
            
            ValueToComplete = taskData.randomValuesToComplete[Random.Range(0, taskData.randomValuesToComplete.Count - 1)];
        }
        
        public DestroyTotemsTask(TaskDataReachValueUlong taskData, MainCharacterData characterData, TaskSaveData taskSaveData)
        {
            TaskType = taskData.taskType;
            RewardData = taskSaveData.rewardData;
            _taskData = taskData;
            CurrentValue = taskSaveData.currentValue;
            ValueToComplete = taskSaveData.valueToComplete;
            IsCompleted = taskSaveData.isCompleted;
            IsRewardClaimed = taskSaveData.isRewardClaimed;
        
            if (!IsCompleted)
            {
                
            }
        }

        public override string GetTitle(LanguageType language)
        {
            return string.Format(_taskData.GetTitle(language), ValueToComplete);
        }

        public override void FillTaskViewImage(Image fillImage)
        {
            fillImage.fillAmount = (float)CurrentValue / ValueToComplete;
        }
    }
}