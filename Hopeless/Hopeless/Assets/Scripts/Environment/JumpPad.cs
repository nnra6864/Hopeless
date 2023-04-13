using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] ParticleSystem _activatedParticles;
        [SerializeField] Vector2 _force;

        private void Awake()
        {
            var main = _activatedParticles.main;
            main.startSpeed = _force.x + _force.y;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb)) return;
            rb.velocity = Vector2.zero;
            rb.AddForce(_force, ForceMode2D.Impulse);
            _activatedParticles.Play();
        }
    }
}