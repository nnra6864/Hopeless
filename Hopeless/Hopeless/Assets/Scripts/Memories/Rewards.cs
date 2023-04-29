using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    [SerializeField] PlayerUpgrades _upgrades;
    [SerializeField] Player.Player _player;
    [SerializeField] RewardPanel _rewardPanel;

    [Header("Reward Messages")]
    [SerializeField] string _groundText, _dashText, _distancePerBounceText;

    public void UnlockGround()
    {
        _upgrades.Ground = true;
        _rewardPanel.RewardText.text = _groundText;
        _rewardPanel.DisplayReward();
    }

    public void UnlockDash()
    {
        _upgrades.Dash = true;
        _rewardPanel.RewardText.text = _dashText;
        _rewardPanel.DisplayReward();
    }

    public void UnlockDistancePerBounce()
    {
        _upgrades.MaxBulletDistancePerBounce = true;
        _rewardPanel.RewardText.text = _distancePerBounceText;
        _rewardPanel.DisplayReward();
    }

    public void IncreaseDamage(int amount)
    {
        _upgrades.Damage += amount;
        _rewardPanel.RewardText.text = $"+{amount} Bullet Damage";
        _rewardPanel.DisplayReward();
    }

    public void IncreaseDistance(float amount)
    {
        _upgrades.MaxBulletDistance += amount;
        _rewardPanel.RewardText.text = $"+{amount} Bullet Distance";
        _rewardPanel.DisplayReward();
    }

    public void IncreaseBounciness(int amount)
    {
        _upgrades.BounceAmount += amount;
        _rewardPanel.RewardText.text = $"+{amount} Bullet Bounces";
        _rewardPanel.DisplayReward();
    }

    public void IncreaseHealth(int amount)
    {
        _player.Sanity += amount;
        _rewardPanel.RewardText.text = $"+{amount} Player Health";
        _rewardPanel.DisplayReward();
    }

    public void IncreaseFirerate(float amount)
    {
        _upgrades.FireRate += amount;
        _rewardPanel.RewardText.text = $"{amount}s Firerate";
        _rewardPanel.DisplayReward();
    }

    public void IncreaseBulletSpeed(float amount)
    {
        _upgrades.BulletSpeed += amount;
        _rewardPanel.RewardText.text = $"+{amount} Bullet Speed";
        _rewardPanel.DisplayReward();
    }
}