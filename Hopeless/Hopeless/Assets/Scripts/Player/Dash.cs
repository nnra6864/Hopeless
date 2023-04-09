using System.Collections;
using UnityEngine;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private Movement _movement;
        [SerializeField] private Jump _jump;
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashCooldown;
        private bool _canDash = true;
        [SerializeField] private ParticleSystem _dashParticles;

        private void Update()
        {
            if ((!Input.GetKeyDown(KeyCode.F) && !Input.GetKeyDown(KeyCode.LeftShift)) || !_canDash) return;
            PerformDash();
        }

        void PerformDash()
        {
            _playerRb.constraints = RigidbodyConstraints2D.FreezePositionY;
            var force = _dashForce * _movement.Direction;
            _playerRb.AddForce(new (force, 0), ForceMode2D.Impulse);
            PlayDashParticles(Mathf.Sign(force));
            StartCoroutine(UnfreezeY());
            StartCoroutine(DashCooldown());
        }

        void PlayDashParticles(float dir)
        {
            var shape = _dashParticles.shape;
            shape.rotation = new (0, 90 * -dir, 0);
            _dashParticles.Play();
        }

        private Coroutine _unfreezeY;
        IEnumerator UnfreezeY()
        {
            yield return new WaitForSeconds(.2f);
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _jump.JumpFuel = _jump.MaxJumpFuel;
        }

        IEnumerator DashCooldown()
        {
            _canDash = false;
            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
        }
    }
}
