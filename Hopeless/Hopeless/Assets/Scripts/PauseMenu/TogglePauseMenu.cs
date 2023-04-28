using System.Collections;
using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    [SerializeField] Transform _menuTransform;
    public static bool IsActive;

    void Update()
    {
        if (Input.GetKeyUp(Prefs.KeyBinds[Prefs.Actions.Pause])) Toggle();
    }

    public bool Toggle()
    {
        if (_lerpMenuRoutine != null) return false;
        _lerpMenuRoutine = StartCoroutine(LerpMenuRoutine(_menuLerpPos == 0 ? 1 : -1));
        return true;
    }

    public void NoCheckToggle()
    {
        if (_lerpMenuRoutine != null) return;
        _lerpMenuRoutine = StartCoroutine(LerpMenuRoutine(_menuLerpPos == 0 ? 1 : -1));
    }

    float _menuLerpPos = 0;
    Coroutine _lerpMenuRoutine;
    IEnumerator LerpMenuRoutine(int dir)
    {
        if (dir == 1) _menuTransform.gameObject.SetActive(true);
        IsActive = dir == 1;
        while (dir == 1 ? _menuLerpPos < 1 : _menuLerpPos > 0)
        {
            _menuLerpPos += (Time.unscaledDeltaTime * dir) / 0.5f;
            _menuLerpPos = Mathf.Clamp01(_menuLerpPos);
            var t = NnUtils.EaseInOutQuad(_menuLerpPos);
            _menuTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            CustomTime.Time = 1 - t;
            yield return null;
        }
        if (dir == -1) _menuTransform.gameObject.SetActive(false);
        _lerpMenuRoutine = null;
    }
}