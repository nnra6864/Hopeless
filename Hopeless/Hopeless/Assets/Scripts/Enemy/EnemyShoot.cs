using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        [SerializeField] int _damage, _bulletSpeed, _bounceAmount;
        [SerializeField] float _lifetime, _fireRate;
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] ParticleSystem _shootParticles;
        [HideInInspector] public List<Transform> TargetPositions = new();

        private void Awake()
        {
            StartCoroutine(Fire());
        }

        public void ActivateEnemy() => StartCoroutine(Fire());

        IEnumerator Fire()
        {
            while (true)
            {
                if (TargetPositions.Count < 1)
                {
                    yield return null;
                    continue;
                }

                Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                bullet.BounceAmount = _bounceAmount;
                bullet.BulletSpeed = _bulletSpeed;
                var fireDir = Vector3.Normalize(TargetPositions[^1].position - transform.position);
                bullet.Direction = fireDir;
                bullet.DamageAmount = _damage;
                bullet.Lifetime = _lifetime;

                _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, fireDir);
                var pMain = _shootParticles.main;
                pMain.startSpeed = _bulletSpeed;
                _shootParticles.Play();

                yield return new WaitForSeconds(_fireRate);
            }
        }
    }
}
