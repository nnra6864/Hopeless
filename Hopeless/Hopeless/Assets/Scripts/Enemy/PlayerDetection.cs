using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class PlayerDetection : MonoBehaviour
    {
        [SerializeField] EnemyShoot _shootScript;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _shootScript._playerDetections++;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _shootScript._playerDetections--;
        }
    }
}