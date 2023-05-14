using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerUpgrades : MonoBehaviour
    {
        public bool Dash;
        public bool Ground;
        public float MaxBulletDistance;
        public bool MaxBulletDistancePerBounce;
        public float FireRate;
        public int Damage;
        public int BounceAmount;
        public float BulletSpeed;

        private void Awake()
        {
            bool nn = PlayerPrefs.GetInt("nn", 0) == 1;
            MaxBulletDistancePerBounce = Dash = Ground = nn;
            MaxBulletDistance = nn ? 50 : 15;
            FireRate = nn ? 0.1f : 1;
            Damage = nn ? 999 : 1;
            BounceAmount = nn ? 5 : 3;
            BulletSpeed = nn ? 50 : 25;
        }
    }
}