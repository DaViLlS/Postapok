using System;

namespace _Project.Rewards
{
    [Serializable]
    public class RewardData
    {
        public ulong itemId;
        public ulong rewardCount;
        public RewardType rewardType;
    }
}