using _Project.Localization;
using _Project.Rewards;

namespace _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec
{
    public class PlotTask : Task
    {
        public string TaskName { get; protected set; }
        public string[] LevelToCompleteNames { get; private set; }
        
        private TaskData _taskData;
        
        public PlotTask(string taskName, TaskType taskType, RewardData rewardData, TaskData taskData)
        {
            TaskName = taskName;
            TaskType = taskType;
            RewardData = rewardData;
            _taskData = taskData;
        }
        
        public PlotTask(string taskName, TaskType taskType, string[] levelToCompleteNames,
            RewardData rewardData, TaskData taskData)
        {
            TaskName = taskName;
            TaskType = taskType;
            LevelToCompleteNames = levelToCompleteNames;
            RewardData = rewardData;
            _taskData = taskData;
        }

        public override string GetTitle(LanguageType language)
        {
            return _taskData.GetTitle(language);
        }
    }
}