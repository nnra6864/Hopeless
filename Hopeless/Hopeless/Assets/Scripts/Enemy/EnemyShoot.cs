using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        [Header("Raycasting")]
        [Range(1, 256)]
        [SerializeField] int _numberOfRays;
        [Range(-360, 360)]
        [SerializeField] float _offset;
        [SerializeField] bool _includeEnemyRotation;
        [HideInInspector] float _detectionAngleStep;
        [SerializeField] float _maxTargetDistance;
        [SerializeField] bool _maxDistancePerBounce;
        [SerializeField] bool _needsPlayerInRange;
        [SerializeField] bool _visualiseRays;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] string _targetTag;
        [HideInInspector] public int _playerDetections;

        [Header("Shooting")]
        [SerializeField] int _damage;
        [SerializeField] int _bounceAmount;
        [SerializeField] float _bulletSpeed, _fireRate, _lifetime;
        [SerializeField] EnemyBullet _bulletPrefab;
        [SerializeField] ParticleSystem _shootParticles;

        private void Awake()
        {
            UpdateAngleStep();
        }

        private void OnValidate()
        {
            if (_numberOfRays == 0) return;
            UpdateAngleStep();
        }

        public void UpdateAngleStep() => _detectionAngleStep = 360 / _numberOfRays;

        private void Update()
        {
            if ((_needsPlayerInRange && _playerDetections < 1) || _fireRoutine != null) return;

            Vector2 ?targetPos = TargetDirection();
            if (targetPos == null) return;
            _fireRoutine = StartCoroutine(Fire((Vector2)targetPos));
        }

        Vector2 ?TargetDirection()
        {
            Dictionary<Vector2, float> targets = new();
            for (int i = 0; i < _numberOfRays; i++)
            {
                var angle = (_detectionAngleStep * i) - _offset;
                angle += _includeEnemyRotation ? transform.rotation.eulerAngles.z : 0;
                var rad = angle * Mathf.Deg2Rad;
                var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                float ?magnitude = CircleCast(dir);
                if (magnitude == null) continue;
                targets.TryAdd(dir, (float)magnitude);
            }
            if (targets.Count < 1)
                return null;
            return targets.OrderBy(v => v.Value).First().Key;
        }

        private float ?CircleCast(Vector2 dir)
        {
            var distance = _maxTargetDistance;
            var bouncesLeft = _bounceAmount + 1;
            Vector2 castOrigin = transform.position;
            float totalMagnitude = 0;
            while (bouncesLeft > 0)
            {
                var hit = Physics2D.CircleCast(castOrigin, 0.25f, dir, distance, _layerMask);
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
                if (hit.transform.CompareTag(_targetTag)) return totalMagnitude;
                dir = Vector2.Reflect(dir, hit.normal);
                castOrigin = hit.point + hit.normal * 0.26f;
                bouncesLeft--;
                if (!_maxDistancePerBounce) distance -= hit.distance;
                distance = Mathf.Clamp(distance, 0, _maxTargetDistance);
            }
            return null;
        }

        Coroutine _fireRoutine;
        IEnumerator Fire(Vector2 direction)
        {
            EnemyBullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.BounceAmount = _bounceAmount;
            bullet.BulletSpeed = _bulletSpeed;
            bullet.Direction = direction;
            bullet.DamageAmount = _damage;
            bullet.MaxDistance = _maxTargetDistance;
            bullet.MaxDistancePerBounce = _maxDistancePerBounce;

            _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            var pMain = _shootParticles.main;
            pMain.startSpeed = _bulletSpeed;
            _shootParticles.Play();

            yield return new WaitForSeconds(_fireRate);
            _fireRoutine = null;
        }
    }
}