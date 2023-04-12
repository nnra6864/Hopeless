using Assets.Scripts;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IHittable
    {
        [SerializeField] private int _startingSanity;
        private int _sanity;
        public int Sanity
        {
            get => _sanity;
            set
            {
                if (value <= 0)
                {
                    _sanity = 0;
                    Die();
                    return;
                }
                _sanity = value;
            }
        }

        private void Awake()
        {
            _sanity = _startingSanity;
        }

        public void GetHit(int damageAmount)
        {
            Sanity -= damageAmount;
        }

        void Die()
        {
            Sanity = _startingSanity;
            transform.position = Vector3.zero;
        }
    }
}