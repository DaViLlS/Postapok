using System;
using _Project.Rewards;
using _Project.TasksAndDialogues.Tasks.Scripts;
using _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec;

namespace _Project.Main.PlayerData.TasksData
{
    [Serializable]
    public class TaskSaveData
    {
        public string taskName;
        public string[] levelToCompleteNames;
        public TaskType taskType;
        public ulong startValue;
        public ulong currentValue;
        public ulong valueToComplete;
        public RewardData rewardData;
        public bool isCompleted;
        public bool isRewardClaimed;
        
        public void Setup(Task task)
        {
            if (task is PlotTask plotTask)
            {
                taskName = plotTask.TaskName;
                levelToCompleteNames = plotTask.LevelToCompleteNames;
            }
            
            taskType = task.TaskType;
            startValue = task.StartValue;
            currentValue = task.CurrentValue;
            valueToComplete = task.ValueToComplete;
            rewardData = task.RewardData;
            isCompleted = task.IsCompleted;
            isRewardClaimed = task.IsRewardClaimed;
        }
    }
}