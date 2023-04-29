using Assets.Scripts;
using Assets.Scripts.Attack;
using Assets.Scripts.Core;
using Cam;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace Player
{
    public class Player : MonoBehaviour, IHittable
    {
        [SerializeField] Image _sanityFill;
        [SerializeField] private int _startingSanity;
        private int _sanity;
        public int Sanity
        {
            get => _sanity;
            set
            {
                if (_nn) return;
                UpdateSanityUI(value);
                if (value < _sanity) SFX.PlaySFX(gameObject, "PlayerHit", Prefs.Instance.SpatialAudio, false, Random.Range(0.9f, 1.1f));
                if (value <= 0)
                {
                    _sanity = 0;
                    Die();
                    return;
                }
                _sanity = value > 100 ? 100 : value;
            }
        }

        void UpdateSanityUI(int sanity)
        {
            if (_lerpSanity != null) StopCoroutine(_lerpSanity);
            _lerpSanity = StartCoroutine(LerpSanity(sanity / 100f));
        }
        Coroutine _lerpSanity;
        IEnumerator LerpSanity(float target)
        {
            float lerpPos = 0;
            var starting = _sanityFill.fillAmount;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / 0.5f;
                lerpPos = Mathf.Clamp01(lerpPos);
                var t = NnUtils.EaseInOutCubic(lerpPos);
                _sanityFill.fillAmount = Mathf.Lerp(starting, target, t);
                yield return null;
            }
            _lerpSanity = null;
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
        [SerializeField] ParticleSystem _playerParticles;

        private void Awake()
        {
            _nn = PlayerPrefs.GetInt("nn", 0) == 1;
            _sanity = _startingSanity;
            _bulletTrajectory.startWidth = Prefs.Instance.TrajectorySize;
            Prefs.Instance.OnTrajectorySizeChanged += () => _bulletTrajectory.startWidth = Prefs.Instance.TrajectorySize;
        }

        public void GetHit(int damageAmount)
        {
            Sanity -= damageAmount;
        }

        public void Die()
        {
            if (_dieRoutine != null) return;
            if (!Prefs.Instance.UseDeathEffect)
            {
                transform.position = CheckPoint;
                Sanity = _startingSanity;
                CameraManager.LerpCameraSize(CheckPointCamSize, 0);
                return;
            }
            ToggleComponents(false);
            _rb.velocity = Vector2.zero;
            _dieRoutine = StartCoroutine(DieRoutine());
        }

        Coroutine _dieRoutine;
        IEnumerator DieRoutine()
        {
            float lerpPos = 0;
            var startingPos = transform.position;
            _deathEffect.Play();
            SFX.PlaySFX(gameObject, "PlayerDeath", Prefs.Instance.SpatialAudio);
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
            _dieRoutine = null;
        }

        public void ToggleComponents(bool active)
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
            _playerParticles.gameObject.SetActive(active);
        }
        bool _nn;
    }
}