using _Project.MainCharacter.Scripts;
using _Project.MainCharacter.Scripts.Leveling;
using Zenject;

namespace _Project.Rewards
{
    public class RewardsController
    {
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private LevelController _levelController;

        public void ApplyReward(RewardType rewardType, ulong rewardAmount, ulong itemId)
        {
            switch (rewardType)
            {
                case RewardType.SoftMoney:
                    _mainCharacterData.AddSoftValue(rewardAmount);
                    break;
                case RewardType.HardMoney:
                    _mainCharacterData.AddHardValue(rewardAmount);
                    break;
                case RewardType.CharacterExperience:
                    _levelController.AddExperience(rewardAmount);
                    break;

            }
        }
    }
}