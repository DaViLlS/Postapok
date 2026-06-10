using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.TasksAndDialogues.Tasks.Scripts.DailyWeekly.Data
{
    [CreateAssetMenu(menuName = "Tasks/DailyTasksConfig", fileName = "DailyTasksConfig")]
    public class DailyTasksConfig : ScriptableObject
    {
        [SerializeField] private int maxDailyTasksCount;
        [SerializeField] private List<TaskType> taskTypes;
        [SerializeField] private List<TaskDataReachCondition> tasksDataReachCondition;
        [SerializeField] private List<TaskDataReachValueUlong> tasksDataReachValueUlong;
        
        public int MaxDailyTasksCount => maxDailyTasksCount;
        public List<TaskType> TaskTypes => taskTypes;
        public List<TaskDataReachCondition> TasksDataReachCondition => tasksDataReachCondition;
        public List<TaskDataReachValueUlong> TasksDataReachValueUlong => tasksDataReachValueUlong;

        public TaskDataReachCondition GetConditionTaskByType(TaskType taskType)
        {
            return tasksDataReachCondition.First(x => x.taskType == taskType);
        }

        public TaskDataReachValueUlong GetUlongTaskByType(TaskType taskType)
        {
            return tasksDataReachValueUlong.First(x => x.taskType == taskType);
        }
    }
}