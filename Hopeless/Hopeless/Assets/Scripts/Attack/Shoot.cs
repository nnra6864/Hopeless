using Assets.Scripts.Core;
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
        [SerializeField] AudioClip _shootSound;
        [Header("Bullet")]
        [SerializeField] Bullet _bulletPrefab;
        public float FireRate;
        public int BounceAmount;
        public float BulletSpeed;
        public float Lifetime;
        public int Damage;

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
            _playerRb.AddForce(-dir * 15, ForceMode2D.Impulse);
        }

        Coroutine _fireRoutine;
        IEnumerator FireRoutine()
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.BounceAmount = BounceAmount;
            bullet.BulletSpeed = BulletSpeed;
            var fireDir = ((Vector2)(_mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
            bullet.Direction = fireDir;
            bullet.DamageAmount = Damage;
            bullet.Lifetime = Lifetime;
            SFX.PlaySFX(gameObject, "PlayerShoot");

            AddKnokback(fireDir);
            _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, fireDir);
            var pMain = _shootParticles.main;
            pMain.startSpeed = BulletSpeed;
            _shootParticles.Play();
            _lineRenderer.positionCount = 0;

            yield return new WaitForSeconds(FireRate);
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
            var bouncesLeft = BounceAmount + 1;
            Vector2 castOrigin = transform.position;
            while (bouncesLeft > 0)
            {
                var hit = Physics2D.CircleCast(castOrigin, 0.25f, dir, Mathf.Infinity, _layerMask);
                if (hit.collider == null)
                {
                    _points.Add((Vector2)transform.position + (dir * 100));
                    return _points;
                }
                _points.Add(hit.point);
                dir = Vector2.Reflect(dir, hit.normal);
                castOrigin = hit.point + hit.normal * 0.26f;
                bouncesLeft--;
            }
            return _points;
        }
        #endregion
    }
}