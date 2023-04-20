using Cam;
using UnityEngine;

namespace Player
{
    public class Checkpoint : MonoBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField] Color _activatedColor;
        [SerializeField] ParticleSystem _particles;
        private Material _mat;
        bool _isUsed;
        private void Awake()
        {
            _mat = GetComponent<Renderer>().material;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isUsed) return;
            var p = collision.GetComponent<Player>();
            p.CheckPoint = transform.position;
            _mat.SetColor("_BaseColor", _activatedColor);
            _mat.SetColor("_OutlineColor", _activatedColor);
            _particles.Play();
            p.CheckPointCamSize = CameraManager.Instance.MainCamera.m_Lens.OrthographicSize;
            _isUsed = true;
        }
    }
}