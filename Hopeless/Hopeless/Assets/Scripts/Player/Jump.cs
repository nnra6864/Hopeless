using UnityEngine;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private float _jumpAcceleration, _fallingGravityMultiplier;
        [SerializeField] public float MaxJumpFuel;
        [HideInInspector] public float JumpFuel;
        private float _gravityScale;

        private void Awake()
        {
            _gravityScale = _playerRb.gravityScale;
            JumpFuel = MaxJumpFuel;
        }

        private void Update()
        {
            if (TogglePauseMenu.IsActive) return;
            ExtraGravity();
            if (_groundCheck.IsGrounded)
            {
                JumpFuel = MaxJumpFuel;
            }
            
            if (!Input.GetKey(Prefs.KeyBinds[Prefs.Actions.Jump]) && !Input.GetKey(Prefs.KeyBinds[Prefs.Actions.JumpSecondary])) return;
            PerformJump();
        }

        void PerformJump()
        {
            if (JumpFuel <= 0) return;
            var accel = _jumpAcceleration * Time.deltaTime;
            JumpFuel -= accel;
            var jumpHeight = JumpFuel < 0 ? accel + JumpFuel : accel;
            JumpFuel = JumpFuel < 0 ? 0 : JumpFuel;
            _playerRb.velocity = _playerRb.velocity.y < 0 ? new Vector2(_playerRb.velocity.x, 0) : _playerRb.velocity;
            _playerRb.velocity += new Vector2(0, jumpHeight);
        }
        
        void ExtraGravity()
        {
            if (_playerRb.velocity.y >= 0)
            {
                _playerRb.gravityScale = _gravityScale;
            }
            else
            {
                _playerRb.gravityScale = _gravityScale * _fallingGravityMultiplier;
            }
        }
    }
}