using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    [SerializeField] PlayerUpgrades _upgrades;
    [SerializeField] Player.Player _player;
    [SerializeField] RewardPanel _rewardPanel;

    [Header("Reward Messages")]
    [TextArea][SerializeField] string _groundText;
    [TextArea][SerializeField] string _dashText;
    [TextArea][SerializeField] string _distancePerBounceText;

    public void UnlockGround()
    {
        _upgrades.Ground = true;
        var text = _groundText.Replace("{1}", Prefs.KeyBinds[Prefs.Actions.Ground].ToString()).Replace("{2}", Prefs.KeyBinds[Prefs.Actions.GroundSecondary].ToString());
        _rewardPanel.DisplayReward(text, 5);
    }

    public void UnlockDash()
    {
        _upgrades.Dash = true;
        var text = _dashText.Replace("{1}", Prefs.KeyBinds[Prefs.Actions.Dash].ToString()).Replace("{2}", Prefs.KeyBinds[Prefs.Actions.DashSecondary].ToString());
        _rewardPanel.DisplayReward(text, 5);
    }

    public void UnlockDistancePerBounce()
    {
        _upgrades.MaxBulletDistancePerBounce = true;
        _rewardPanel.DisplayReward(_distancePerBounceText, 5);
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