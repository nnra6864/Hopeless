using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (value < 0)
                {
                    _health = 0;
                    Die();
                    return;
                }
                _health = value;
            }
        }

        public void GetHit(int damageAmount)
        {
            Health -= damageAmount;
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}