using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Attack
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] Camera _mainCam;
        [SerializeField] ParticleSystem _shootParticles;
        [SerializeField] Rigidbody2D _playerRb;
        [Header("Bullet")]
        [SerializeField] Bullet _bulletPrefab;
        public float FireRate;
        public int BounceAmount;
        public float BulletSpeed;
        public float Lifetime;
        public int Damage;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Return))
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
            var fireDir = Vector3.Normalize(_mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            bullet.Direction = fireDir;
            bullet.DamageAmount = Damage;
            bullet.Lifetime = Lifetime;

            AddKnokback(fireDir);
            _shootParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, fireDir);
            var pMain = _shootParticles.main;
            pMain.startSpeed = BulletSpeed;
            _shootParticles.Play();

            yield return new WaitForSeconds(FireRate);
            _fireRoutine = null;
        }
    }
}