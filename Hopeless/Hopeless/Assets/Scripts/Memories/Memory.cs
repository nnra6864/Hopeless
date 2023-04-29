using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Assets.Scripts.Memories
{
    public class Memory : MonoBehaviour
    {
        [SerializeField] VisualEffect _transition;
        [SerializeField] SpriteRenderer _badMemory, _goodMemory;
        [SerializeField] float _transitionTime;
        [SerializeField] UnityEvent OnMemoryRecovered;

        public void Reveal()
        {
            if (Prefs.Instance.UseDeathEffect)
                StartCoroutine(RevealRoutine());
            else
                StartCoroutine(FadeRoutine());
        }

        IEnumerator RevealRoutine()
        {
            float lerpPos = 0;
            _transition.gameObject.SetActive(true);
            _transition.Play();
            yield return null;
            _badMemory.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / _transitionTime;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseOutIn(lerpPos);
                float t2 = NnUtils.EaseInQuad(lerpPos);
                _transition.SetFloat("Transition Position", t);
                _transition.SetFloat("Blend Position", t2);
                yield return null;
            }
            lerpPos = 0;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / 0.25f;
                lerpPos = Mathf.Clamp01(lerpPos);
                _goodMemory.color = new Color(_goodMemory.color.r, _goodMemory.color.g, _goodMemory.color.b, lerpPos);
            }
            OnMemoryRecovered?.Invoke();
            yield return new WaitForSeconds(3f);
            _transition.gameObject.SetActive(false);
        }

        IEnumerator FadeRoutine()
        {
            float lerpPos = 0;
            _goodMemory.color = new(_goodMemory.color.r, _goodMemory.color.g, _goodMemory.color.b, 0);
            yield return null;
            _goodMemory.gameObject.SetActive(true);
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / _transitionTime;
                lerpPos = Mathf.Clamp01(lerpPos);
                float t = NnUtils.EaseInOut(lerpPos);
                _badMemory.color = new(_badMemory.color.r, _badMemory.color.g, _badMemory.color.b, 1 - t);
                _goodMemory.color = new(_goodMemory.color.r, _goodMemory.color.g, _goodMemory.color.b, t);
                yield return null;
            }
            _badMemory.gameObject.SetActive(false);
            OnMemoryRecovered?.Invoke();
        }
    }
}