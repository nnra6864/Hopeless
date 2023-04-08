using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _playerColldier;
        [SerializeField] private LayerMask _ground;
        public bool IsGrounded()
        {
            if (Physics2D.CircleCast(_playerColldier.bounds.center, _playerColldier.radius, Vector2.down, .1f, _ground))
                return true;
            return false;
        }
    }
}
