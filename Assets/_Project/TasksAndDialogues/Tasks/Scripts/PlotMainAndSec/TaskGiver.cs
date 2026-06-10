using _Project.TasksAndDialogues.Dialogues.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using Zenject;

namespace _Project.TasksAndDialogues.Tasks.Scripts.PlotMainAndSec
{
    public class TaskGiver : MonoBehaviour, IInteractable
    {
        [Inject] private TasksController _tasksController;

        [SerializeField] private GameObject taskSign;
        [SerializeField] private TaskData taskData;
        [SerializeField] private DialoguesController dialoguesController;
        [SerializeField] private DialogueConfig dialogueConfig;

        private bool _isDialogueStarted;

        private void Start()
        {
            foreach (var task in _tasksController.CurrentMainPlotTasks)
            {
                var plotTask = task as PlotTask;

                if (plotTask != null && plotTask.TaskName == taskData.taskName)
                {
                    taskSign.SetActive(false);
                    return;
                }
            }
            
            foreach (var task in _tasksController.CurrentSecondaryPlotTasks)
            {
                var plotTask = task as PlotTask;

                if (plotTask != null && plotTask.TaskName == taskData.taskName)
                {
                    taskSign.SetActive(false);
                    return;
                }
            }
        }

        public void Interact()
        {
            foreach (var task in _tasksController.CurrentMainPlotTasks)
            {
                var plotTask = task as PlotTask;

                if (plotTask != null && plotTask.TaskName == taskData.taskName)
                {
                    return;
                }
            }
            
            foreach (var task in _tasksController.CurrentSecondaryPlotTasks)
            {
                var plotTask = task as PlotTask;

                if (plotTask != null && plotTask.TaskName == taskData.taskName)
                {
                    return;
                }
            }
            
            if (_isDialogueStarted)
                return;
            
            _isDialogueStarted = true;
            dialoguesController.OnDialogueCompleted += AddTask;
            dialoguesController.StartDialogue(dialogueConfig);
        }

        private void AddTask()
        {
            dialoguesController.OnDialogueCompleted -= AddTask;
            taskSign.SetActive(false);
            _tasksController.AddSecondaryTask(taskData);
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}