using UnityEngine;

namespace Player
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private float _groundingForce;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) GetGrounded();
        }

        void GetGrounded()
        {
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _playerRb.AddForce(new(0, -_groundingForce), ForceMode2D.Impulse);
        }
    }
}
