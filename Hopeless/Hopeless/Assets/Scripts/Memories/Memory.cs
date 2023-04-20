using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts.Memories
{
    public class Memory : MonoBehaviour
    {
        [SerializeField] VisualEffect _transition;
        [SerializeField] SpriteRenderer _badMemory, _goodMemory;
        [SerializeField] float _transitionTime;

        public void Reveal()
        {
            StartCoroutine(RevealRoutine());
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
            yield return new WaitForSeconds(3f);
            _transition.gameObject.SetActive(false);
        }
    }
}