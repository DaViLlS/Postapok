using System.Collections.Generic;
using UnityEngine;

namespace _Project.Rewards
{
    [CreateAssetMenu(fileName = "RewardsIconConfig", menuName = "Tasks/RewardsIconConfig", order = 0)]
    public class RewardsIconConfig : ScriptableObject
    {
        [SerializeField] private List<RewardIconData> rewardsIcons;

        public Sprite GetIconByType(RewardType rewardType)
        {
            return rewardsIcons.Find(icon => icon.rewardType == rewardType)?.rewardIcon;
        }
    }
}