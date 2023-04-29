using Assets.Scripts.Core;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Attack
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] Camera _mainCam;
        [SerializeField] ParticleSystem _shootParticles;
        [SerializeField] Rigidbody2D _playerRb;
        [SerializeField] LineRenderer _lineRenderer;
        [SerializeField] LayerMask _layerMask;
        [Header("Bullet")]
        [SerializeField] Bullet _bulletPrefab;
        [SerializeField] PlayerUpgrades _playerUpgrades;

        private void Update()
        {
            _lineRenderer.positionCount = 0;
            if (TogglePauseMenu.IsActive) return;
            if ((Input.GetKey(Prefs.KeyBinds[Prefs.Actions.Shoot]) || Input.GetKey(Prefs.KeyBinds[Prefs.Actions.ShootSecondary])) && _fireRoutine == null) DrawTrajectory();
            if ((Input.GetKeyUp(Prefs.KeyBinds[Prefs.Actions.Shoot]) || Input.GetKeyUp(Prefs.KeyBinds[Prefs.Actions.ShootSecondary])) && Time.timeScale == 1)
                Fire();
        }

        void Fire()
        {
            if (_fireRoutine != null) return;
            _fireRoutine = StartCoroutine(FireRoutine());
        }

        void AddKnokback(Vector3 dir)
        {
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _playerRb.velocity = Vector2.zero;
            _playerRb.AddForce(-dir * (_playerUpgrades.BulletSpeed / 2), ForceMode2D.Impulse);
        }

        Coroutine _fireRoutine;
        IEnumerator FireRoutine()
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.BounceAmount = _playerUpgrades.BounceAmount;
            bullet.BulletSpeed = _playerUpgrades.BulletSpeed;
            var fireDir = ((Vector2)(_mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
            bullet.Direction = fireDir;
            bullet.DamageAmount = _playerUpgrades.Damage;
            bullet.MaxDistance = _playerUpgrades.MaxBulletDistance;
            bullet.MaxDistancePerBounce = _playerUpgrades.MaxBulletDistancePerBounce;
            SFX.PlaySFX(gameObject, "PlayerShoot", Prefs.Instance.SpatialAudio);

            AddKnokback(fireDir);
            _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, fireDir);
            var pMain = _shootParticles.main;
            pMain.startSpeed = _playerUpgrades.BulletSpeed;
            _shootParticles.Play();
            _lineRenderer.positionCount = 0;

            yield return new WaitForSeconds(_playerUpgrades.FireRate);
            _fireRoutine = null;
        }

        #region Trajectory
        void DrawTrajectory()
        {
            var dir = ((Vector2)(_mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
            var points = GetTrajectoryPoints(dir);
            _lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                _lineRenderer.SetPosition(i, points[i]);
            }
        }
        private List<Vector2> GetTrajectoryPoints(Vector2 dir)
        {
            List<Vector2> _points = new() { transform.position };
            var bouncesLeft = _playerUpgrades.BounceAmount + 1;
            Vector2 castOrigin = transform.position;
            float distance = _playerUpgrades.MaxBulletDistance;
            while (bouncesLeft > 0)
            {
                var hit = Physics2D.CircleCast(castOrigin, 0.25f, dir, distance, _layerMask);
                if (hit.collider == null)
                {
                    _points.Add(castOrigin + (dir * distance));
                    return _points;
                }
                _points.Add(hit.point);
                dir = Vector2.Reflect(dir, hit.normal);
                castOrigin = hit.point + hit.normal * 0.26f;
                bouncesLeft--;
                if (!_playerUpgrades.MaxBulletDistancePerBounce) distance -= hit.distance;
                distance = Mathf.Clamp(distance, 0, _playerUpgrades.MaxBulletDistance);
            }
            return _points;
        }
        #endregion
    }
}