using System.Collections.Generic;
using UnityEngine;

namespace _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec
{
    [CreateAssetMenu(menuName = "Tasks/MainTasksConfig", fileName = "MainTasksConfig")]
    public class MainTasksConfig : ScriptableObject
    {
        [SerializeField] private List<TaskData> mainTasks;
        [SerializeField] private List<TaskData> secondaryTasks;

        public List<TaskData> MainTasks => mainTasks;
        public List<TaskData> SecondaryTasks => secondaryTasks;
    }
}