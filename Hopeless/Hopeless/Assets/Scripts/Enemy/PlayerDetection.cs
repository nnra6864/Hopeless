using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class PlayerDetection : MonoBehaviour
    {
        [SerializeField] EnemyShoot _shootScript;
        [SerializeField] Transform _target;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _shootScript.TargetPositions.Add(_target);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_shootScript.TargetPositions.Contains(_target))
                _shootScript.TargetPositions.Remove(_target);
        }
    }
}