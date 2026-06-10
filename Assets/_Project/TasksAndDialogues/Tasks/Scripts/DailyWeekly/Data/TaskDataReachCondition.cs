using System;
using _Project.Localization;

namespace _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Data
{
    [Serializable]
    public class TaskDataReachCondition
    {
        public TaskType taskType;
        public string title;
        public TextData[] titles;
        
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