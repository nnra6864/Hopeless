using Assets.Scripts.Core;
using Assets.Scripts.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Assets.Scripts.Enemy
{
    public class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] private int _startingHealth;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] VisualEffect _deathEffect;
        [SerializeField] string _hitSFX, _deathSFX;
        [Header("Components")]
        [SerializeField] SpriteRenderer _renderer;
        [SerializeField] EnemyShoot _shoot;
        [SerializeField] Collider2D _coll;
        [SerializeField] Rigidbody2D _rb;

        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                _hitParticles.Play();
                if (value <= 0)
                {
                    _health = 0;
                    Die();
                    return;
                }
                SFX.PlaySFX(gameObject, _hitSFX, Prefs.Instance.SpatialAudio, true, Random.Range(0.75f, 1.25f));
                _health = value;
            }
        }
        [SerializeField] UnityEvent _onDeath;
        [SerializeField] List<TransformObject.TransformStruct> _transformObjects = new();

        void Awake() => _health = _startingHealth;

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
            ToggleComponents(false);
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            if (!Prefs.Instance.UseDeathEffect) goto Destroy;
            _deathEffect.SetVector4("Color", (Vector4)(Color)_renderer.color * 2);
            _deathEffect.Play();
            SFX.PlaySFX(gameObject, _deathSFX, Prefs.Instance.SpatialAudio, true);
            yield return new WaitForSeconds(2.1f);
            Destroy:
            Destroy(gameObject);
        }

        void ToggleComponents(bool active)
        {
            _renderer.enabled = active;
            _shoot.enabled = active;
            _coll.enabled = active;
            if (_rb != null) _rb.simulated = active;
        }
    }
}