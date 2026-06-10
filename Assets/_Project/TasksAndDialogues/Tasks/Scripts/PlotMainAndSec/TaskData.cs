using _Project.Localization;
using _Project.Rewards;
using UnityEngine;

namespace _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec
{
    [CreateAssetMenu(menuName = "Tasks/TaskConfig", fileName = "TaskConfig")]
    public class TaskData : ScriptableObject
    {
        public string taskName;
        public TextData[] taskNames;
        public RewardData rewardData;
        
        public string GetTitle(LanguageType language)
        {
            for (var i = 0; i < taskNames.Length; i++)
            {
                if (taskNames[i].language == language)
                {
                    return taskNames[i].text;
                }
            }
            
            return string.Empty;
        }
    }
}