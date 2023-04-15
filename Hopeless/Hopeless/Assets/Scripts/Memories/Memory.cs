using UnityEngine;

namespace Assets.Scripts.Memories
{
    public class Memory : MonoBehaviour
    {
        [SerializeField] GameObject _badMemory, _goodMemory;

        public void Reveal()
        {
            _badMemory.SetActive(false);
            _goodMemory.SetActive(true);
        }
    }
}