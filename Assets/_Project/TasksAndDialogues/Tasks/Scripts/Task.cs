using System;
using _Project.Localization;
using _Project.Rewards;
using UnityEngine.UI;

namespace _Project.TasksAndDialogues.Tasks.Scripts
{
    public class Task
    {
        public event Action<Task> OnTaskConditionChanged;
        public event Action<Task> OnTaskCompleted;

        public TaskType TaskType { get; protected set; }
        public RewardData RewardData { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public bool IsRewardClaimed { get; protected set; }
        public ulong StartValue { get; protected set; }
        public ulong CurrentValue { get; protected set; }
        public ulong ValueToComplete { get; protected set; }

        public virtual string GetTitle(LanguageType language)
        {
            return string.Empty;
        }

        public virtual void FillTaskViewImage(Image fillImage)
        {
            
        }
        
        public virtual void IncCurrentValue()
        {
            CurrentValue++;
            
            CheckCompleteCondition();
        }

        protected virtual void CheckCompleteCondition()
        {
            if (CurrentValue >= ValueToComplete)
            {
                CompleteTask();
            }
            
            UpdateTaskCondition();
        }

        public void UpdateTaskCondition()
        {
            OnTaskConditionChanged?.Invoke(this);
        }

        public void ClaimReward()
        {
            IsRewardClaimed = true;
        }

        public virtual void CompleteTask()
        {
            if (IsCompleted)
                return;

            IsCompleted = true;
            OnTaskCompleted?.Invoke(this);
        }
    }
}