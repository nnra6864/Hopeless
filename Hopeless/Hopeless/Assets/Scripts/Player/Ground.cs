using UnityEngine;

namespace Player
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private float _groundingForce;
        [SerializeField] private ParticleSystem _groundingParticles;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftControl)) GetGrounded();
        }

        void GetGrounded()
        {
            _playerRb.constraints = RigidbodyConstraints2D.None;
            _playerRb.velocity = new(0, _playerRb.velocity.y);
            _playerRb.AddForce(new(0, -_groundingForce), ForceMode2D.Impulse);
            _groundingParticles.Play();
        }
    }
}
