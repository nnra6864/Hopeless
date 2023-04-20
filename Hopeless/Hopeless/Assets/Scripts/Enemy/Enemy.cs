using Assets.Scripts.Environment;
using System.Collections.Generic;
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
        [SerializeField] List<TransformObject.TransformStruct> _transformObjects = new();

        public void GetHit(int damageAmount)
        {
            Health -= damageAmount;
        }

        void Die()
        {
            foreach(var t in _transformObjects)
                if (t.ObjectScript != null)
                    t.Execute();
            _onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}