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
        _rewardPanel.DisplayReward(_groundText);
    }

    public void UnlockDash()
    {
        _upgrades.Dash = true;
        _rewardPanel.DisplayReward(_dashText);
    }

    public void UnlockDistancePerBounce()
    {
        _upgrades.MaxBulletDistancePerBounce = true;
        _rewardPanel.DisplayReward(_distancePerBounceText);
    }

    public void IncreaseDamage(int amount)
    {
        _upgrades.Damage += amount;
        _rewardPanel.DisplayReward($"+{amount} Bullet Damage");
    }

    public void IncreaseDistance(float amount)
    {
        _upgrades.MaxBulletDistance += amount;
        _rewardPanel.DisplayReward($"+{amount} Bullet Distance");
    }

    public void IncreaseBounciness(int amount)
    {
        _upgrades.BounceAmount += amount;
        _rewardPanel.DisplayReward($"+{amount} Bullet Bounces");
    }

    public void IncreaseHealth(int amount)
    {
        _player.Sanity += amount;
        _rewardPanel.DisplayReward($"+{amount} Player Health");
    }

    public void IncreaseFirerate(float amount)
    {
        _upgrades.FireRate += amount;
        _rewardPanel.DisplayReward($"{amount}s Firerate");
    }

    public void IncreaseBulletSpeed(float amount)
    {
        _upgrades.BulletSpeed += amount;
        _rewardPanel.DisplayReward($"+{amount} Bullet Speed");
    }
}