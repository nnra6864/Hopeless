using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ButtonSelection : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _play, _settings, _quit;
    private Image _currentImage, _targetImage;
    private VisualEffect _currentEffect;
    private GameObject _newTarget;

    private void Awake()
    {
        _currentImage = _play.GetComponent<Image>();
    }

    public void SetTarget(GameObject obj)
    {
        if (_revealRoutine != null)
        {
            _newTarget = obj;
            return;
        }
        Reveal(obj);
    }

    public void ClearNewTarget()
    {
        _newTarget = null;
    }

    void Reveal(GameObject target)
    {
        if (target.gameObject == _currentImage.gameObject) return;
        _targetImage = target.GetComponent<Image>();
        _currentEffect = GetEffect(target);
        _revealRoutine = StartCoroutine(RevealRoutine());
    }

    VisualEffect GetEffect(GameObject target)
    {
        if (target == _play)
            if (_currentImage.gameObject == _settings)
                return target.transform.GetChild(0).GetComponent<VisualEffect>();
            else return target.transform.GetChild(1).GetComponent<VisualEffect>();
        else if (target == _settings)
            if (_currentImage.gameObject == _play)
                return target.transform.GetChild(0).GetComponent<VisualEffect>();
            else return target.transform.GetChild(1).GetComponent<VisualEffect>();
        else if (target == _quit)
            if (_currentImage.gameObject == _play)
                return target.transform.GetChild(0).GetComponent<VisualEffect>();
            else return target.transform.GetChild(1).GetComponent<VisualEffect>();
        return null;
    }

    Coroutine _revealRoutine;
    IEnumerator RevealRoutine()
    {
        float lerpPos = 0;
        _currentEffect.enabled = true;
        _currentEffect.Play();
        yield return new WaitForSeconds(0.1f);
        _currentImage.transform.GetChild(2).gameObject.SetActive(false);
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 0.3f;
            lerpPos = Mathf.Clamp01(lerpPos);
            float t = NnUtils.EaseOutQuad(lerpPos);
            _currentEffect.SetFloat("Blend Position", t);
            yield return null;
        }
        _targetImage.transform.GetChild(2).gameObject.SetActive(true);
        _currentImage = _targetImage;
        _targetImage = null;
        yield return new WaitForSeconds(0.1f);
        _revealRoutine = null;
        _currentEffect.enabled = false;
        _currentEffect.SetFloat("Blend Position", 0);
        yield return null;
        if (_newTarget == null) yield break;
        Reveal(_newTarget);
        _newTarget = null;
    }
}