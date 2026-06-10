using System;
using _Project.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.TasksAndDialogues.Tasks.Scripts.UI
{
    public class TaskElement : MonoBehaviour
    {
        public event Action<TaskElement> OnRewardClaimed;
        
        [Inject] private TasksController _tasksController;
        
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image fillImage;
        [SerializeField] private Button claimButton;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TMP_Text rewardText;
        
        private RewardData _rewardData;
        
        public Task Task { get; private set; }
        public TaskType TaskType { get; private set; }
        
        public void Setup(Task task,string titleText, bool canClaim, TaskType taskType, RewardData rewardData, Sprite rewardSprite)
        {
            Task = task;
            title.text = titleText;
            claimButton.gameObject.SetActive(canClaim);
            TaskType = taskType;
            rewardImage.sprite = rewardSprite;
            _rewardData = rewardData;
            rewardText.text = _rewardData.rewardCount.ToString();
        }
        
        public void Setup(Task task,string titleText, bool canClaim, TaskType taskType)
        {
            Task = task;
            title.text = titleText;
            TaskType = taskType;
            rewardText.gameObject.SetActive(false);
            rewardImage.gameObject.SetActive(false);
            claimButton.gameObject.SetActive(false);
        }

        public void UpdateView(string titleText, bool canClaim)
        {
            title.text = titleText;
            
            if (_rewardData != null)
            {
                claimButton.gameObject.SetActive(canClaim);
            }
        }

        public void ClaimReward()
        {
            Task.ClaimReward();
            _tasksController.MakeReward(TaskType, _rewardData);
            OnRewardClaimed?.Invoke(this);
        }
        
        public Image GetFillImage() => fillImage;
    }
}