using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Environment
{
    public class TriggerEnterEvent : MonoBehaviour
    {
        [SerializeField] private List<UnityEvent> _onButtonActivated;
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
            _numberOfUses++;
            _renderer.material.SetColor("_BaseColor", _activatedColor);
            _renderer.material.SetColor("_OutlineColor", _activatedColor);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_multiActivate) return;
            _renderer.material.SetColor("_BaseColor", _deactivatedColor);
            _renderer.material.SetColor("_OutlineColor", _deactivatedColor);
        }
    }
}