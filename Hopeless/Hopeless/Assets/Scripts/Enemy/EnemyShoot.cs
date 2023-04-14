using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        [Header("Raycasting")]
        [Range(8, 128)]
        [SerializeField] int _numberOfRays;
        [HideInInspector] float _detectionAngleStep;
        [SerializeField] float _maxTargetDistance;
        [SerializeField] bool _maxDistancePerRay;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] bool _visualiseRays;

        [SerializeField] int _damage, _bulletSpeed, _bounceAmount;
        [SerializeField] float _lifetime, _fireRate;
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] ParticleSystem _shootParticles;
        [HideInInspector] public int _playerDetections;

        private void Awake()
        {
            _detectionAngleStep = 360 / _numberOfRays;
        }

        private void Update()
        {
            if (_playerDetections < 1 || _fireRoutine != null) return;

            Vector2 ?targetPos = TargetDirection();
            if (targetPos == null) return;
            _fireRoutine = StartCoroutine(Fire((Vector2)targetPos));
        }

        Vector2 ?TargetDirection()
        {
            Dictionary<Vector2, float> targets = new();
            for (int i = 0; i < _numberOfRays; i++)
            {
                var rad = (_detectionAngleStep * i) * Mathf.Deg2Rad;
                var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                float ?magnitude = RayCast(dir);
                if (magnitude == null) continue;
                targets.TryAdd(dir, (float)magnitude);
            }
            if (targets.Count < 1)
                return null;
            return targets.OrderBy(v => v.Value).First().Key;
        }

        private float ?RayCast(Vector2 dir)
        {
            var distance = _maxTargetDistance;
            var bouncesLeft = _bounceAmount + 1;
            Vector2 castOrigin = transform.position;
            float totalMagnitude = 0;
            while (bouncesLeft > 0)
            {
                var hit = Physics2D.Raycast(castOrigin, dir, distance, _layerMask);
                if (hit.collider == null)
                {
                    if (_visualiseRays) Debug.DrawRay(castOrigin, dir * distance);
                    return null;
                }
                if (_visualiseRays)
                {
                    Debug.DrawLine(hit.point, hit.point + (hit.normal / 2), Color.green);
                    Debug.DrawRay(castOrigin, dir * hit.distance);
                }
                totalMagnitude += hit.distance;
                if (hit.transform.CompareTag("Player")) return totalMagnitude;
                dir = Vector2.Reflect(dir, hit.normal);
                castOrigin = hit.point + hit.normal * 0.01f;
                bouncesLeft--;
                if (!_maxDistancePerRay) distance -= hit.distance;
            }
            return null;
        }


        Coroutine _fireRoutine;
        IEnumerator Fire(Vector2 direction)
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.BounceAmount = _bounceAmount;
            bullet.BulletSpeed = _bulletSpeed;
            bullet.Direction = direction;
            bullet.DamageAmount = _damage;
            bullet.Lifetime = _lifetime;

            _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            var pMain = _shootParticles.main;
            pMain.startSpeed = _bulletSpeed;
            _shootParticles.Play();

            yield return new WaitForSeconds(_fireRate);
            _fireRoutine = null;
        }
    }
}