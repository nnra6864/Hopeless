using Assets.Scripts;
using Assets.Scripts.Attack;
using Cam;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

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
        [HideInInspector] public Vector3 CheckPoint = Vector3.zero;
        [HideInInspector] public float CheckPointCamSize = 10;

        [Header("Components")]
        [SerializeField] SpriteRenderer _sRenderer;
        [SerializeField] TrailRenderer _trailRenderer;
        [SerializeField] LineRenderer _bulletTrajectory;
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] CircleCollider2D _coll;
        [SerializeField] Movement _movement;
        [SerializeField] Jump _jump;
        [SerializeField] Dash _dash;
        [SerializeField] Ground _ground;
        [SerializeField] Shoot _shoot;
        [SerializeField] VisualEffect _deathEffect;

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
            ToggleComponents(false);
            _rb.velocity = Vector2.zero;
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            float lerpPos = 0;
            var startingPos = transform.position;
            _deathEffect.Play();
            yield return new WaitForSeconds(1);
            CameraManager.LerpCameraSize(CheckPointCamSize, 3);
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / 3;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseOutIn(lerpPos);
                float t2 = NnUtils.EaseInCubic(lerpPos);
                transform.position = Vector3.Lerp(startingPos, CheckPoint, t2);
                _deathEffect.SetFloat("Transition Position", t);
                _deathEffect.SetFloat("Blend Position", t2);
                yield return null;
            }
            yield return new WaitForSeconds(0.25f);
            Sanity = _startingSanity;
            _deathEffect.Stop();
            _deathEffect.SetFloat("Transition Position", 0);
            _deathEffect.SetFloat("Blend Position", 0);
            ToggleComponents(true);
        }

        void ToggleComponents(bool active)
        {
            _sRenderer.enabled = active;
            _trailRenderer.enabled = active;
            _rb.simulated = active;
            _coll.enabled = active;
            _movement.enabled = active;
            _jump.enabled = active;
            _dash.enabled = active;
            _ground.enabled = active;
            _shoot.enabled = active;
            _bulletTrajectory.positionCount = 0;
            _deathEffect.enabled = !active;
        }
    }
}