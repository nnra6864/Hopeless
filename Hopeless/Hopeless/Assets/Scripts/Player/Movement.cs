using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private float _speed, _maxSpeed;
        [HideInInspector] public int Direction = 1;
        private void Update()
        {
            int direction = 0;
            if (Input.GetKey(KeyCode.A)) direction += -1;
            if (Input.GetKey(KeyCode.D)) direction += 1;
            Move(direction);
            ApplyFriction();
            Direction = direction == 0 ? Direction : direction;
        }

        void Move(int dir)
        {
            var vel = _playerRb.velocity.x;
            if (vel > _maxSpeed || vel < -_maxSpeed) return;
            vel += dir * _speed * Time.deltaTime;
            vel = vel > _maxSpeed ? _maxSpeed : vel < -_maxSpeed ? -_maxSpeed : vel;
            _playerRb.velocity = new(vel, _playerRb.velocity.y);
        }

        void ApplyFriction()
        {
            float amount = Mathf.Min(Mathf.Abs(_playerRb.velocity.x), .2f);
            amount *= Mathf.Sign(_playerRb.velocity.x) * 175 * Time.deltaTime;
            _playerRb.AddForce(new (-amount, 0), ForceMode2D.Impulse);
        }
    }
}
