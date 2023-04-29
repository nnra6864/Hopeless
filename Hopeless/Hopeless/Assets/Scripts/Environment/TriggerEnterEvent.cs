using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Environment
{
    public class TriggerEnterEvent : MonoBehaviour
    {
        [SerializeField] private List<UnityEvent> _onButtonActivated;
        [SerializeField] List<TransformObject.TransformStruct> _transformObjectsOnActivated = new();
        [SerializeField] private bool _multiActivate;
        [SerializeField] private SpriteRenderer _renderer;
        [ColorUsage(true, true)]
        [SerializeField] private Color32 _deactivatedColor, _activatedColor;
        int _numberOfUses;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_multiActivate && _numberOfUses > 1) return;
            foreach (var func in _onButtonActivated)
                func?.Invoke();
            foreach (var t in _transformObjectsOnActivated)
                if (t.ObjectScript != null)
                    t.Execute();
            _numberOfUses++;
            _renderer.material.SetColor("_BaseColor", _activatedColor);
            _renderer.material.SetColor("_OutlineColor", _activatedColor);
            SFX.PlaySFX(gameObject, "Trigger", Prefs.Instance.SpatialAudio, false, Random.Range(0.75f, 1.25f));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_multiActivate) return;
            _renderer.material.SetColor("_BaseColor", _deactivatedColor);
            _renderer.material.SetColor("_OutlineColor", _deactivatedColor);
        }
    }
}