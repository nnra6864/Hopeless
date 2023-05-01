using Assets.Scripts.Core;
using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] PlayerUpgrades _playerUpgrades;
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private Movement _movement;
        [SerializeField] private Jump _jump;
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashCooldown;
        private bool _canDash = true;
        [SerializeField] private ParticleSystem _dashParticles;

        private void Update()
        {
            if (TogglePauseMenu.IsActive || !_playerUpgrades.Dash) return;
            if ((!Input.GetKeyDown(Prefs.KeyBinds[Prefs.Actions.Dash]) && !Input.GetKeyDown(Prefs.KeyBinds[Prefs.Actions.DashSecondary])) || !_canDash) return;
            PerformDash();
        }

        void PerformDash()
        {
            SFX.PlaySFX(gameObject, "Dash", Prefs.Instance.SpatialAudio, false, Random.Range(0.75f, 1.25f));
            var force = _dashForce * _movement.Direction;
            _playerRb.velocity = new(_playerRb.velocity.x, 5);
            _playerRb.AddForce(new (force, 0), ForceMode2D.Impulse);
            PlayDashParticles(Mathf.Sign(force));
            _jump.JumpFuel = _jump.MaxJumpFuel;
            StartCoroutine(DashCooldown());
        }

        void PlayDashParticles(float dir)
        {
            var shape = _dashParticles.shape;
            shape.rotation = new (0, 90 * -dir, 0);
            _dashParticles.Play();
        }

        IEnumerator DashCooldown()
        {
            _canDash = false;
            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
        }
    }
}