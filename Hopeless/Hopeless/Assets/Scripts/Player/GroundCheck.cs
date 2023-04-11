using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _playerColldier;
        [SerializeField] private LayerMask _ground;
        public HashSet<Collider2D> collisions = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((_ground & (1 << collision.gameObject.layer)) == 0) return;
            collisions.Add(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((_ground & (1 << collision.gameObject.layer)) == 0) return;
            collisions.Remove(collision);
        }

        public bool IsGrounded => collisions.Count > 0;
    }
}
