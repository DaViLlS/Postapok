using System;
using System.Collections.Generic;
using _Project.Localization;
using _Project.Rewards;

namespace _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Data
{
    [Serializable]
    public class TaskDataReachValueUlong
    {
        public TaskType taskType;
        public string title;
        public TextData[] titles;
        public List<ulong> randomValuesToComplete;
        public List<RewardData> randomRewards;
        
        public string GetTitle(LanguageType language)
        {
            for (var i = 0; i < titles.Length; i++)
            {
                if (titles[i].language == language)
                {
                    return titles[i].text;
                }
            }
            
            return string.Empty;
        }
    }
}