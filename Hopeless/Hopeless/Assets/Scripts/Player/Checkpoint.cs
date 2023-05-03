using Assets.Scripts.Core;
using Assets.Scripts.Environment;
using Cam;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Checkpoint : MonoBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField] Color _activatedColor;
        [SerializeField] ParticleSystem _particles;
        [SerializeField] UnityEvent _onActivated;
        [SerializeField] List<TransformObject.TransformStruct> _transformObjets;
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
            SFX.PlaySFX(gameObject, "Checkpoint", Prefs.Instance.SpatialAudio, true, Random.Range(0.9f, 1.1f));
            p.CheckPointCamSize = CameraManager.Instance.MainCamera.m_Lens.OrthographicSize;
            _isUsed = true;
            _onActivated?.Invoke();
            foreach (var obj in _transformObjets)
                obj.Execute();
        }
    }
}