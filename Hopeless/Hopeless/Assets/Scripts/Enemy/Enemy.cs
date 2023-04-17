using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Enemy
{
    public class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] private int _startingHealth;
        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                if (value <= 0)
                {
                    _health = 0;
                    Die();
                    return;
                }
                _health = value;
            }
        }
        [SerializeField] UnityEvent _onDeath;
        [SerializeField] UnityEvent<float, float, float, float> _onDeathTransform;

        public void GetHit(int damageAmount)
        {
            Health -= damageAmount;
        }

        void Die()
        {
            _onDeath?.Invoke();
            _onDeathTransform?.Invoke(0, 0, 0, 0);
            Destroy(gameObject);
        }
    }
}