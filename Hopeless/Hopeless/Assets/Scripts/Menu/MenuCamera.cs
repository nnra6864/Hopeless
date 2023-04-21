using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] Camera _mainCam;
    [SerializeField] Vector3 _menuRotation, _generalRotation, _gameplayRotation, _keybindsRotation;
    Quaternion _currentRotation = Quaternion.Euler(Vector3.zero);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToPanel(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToPanel(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToPanel(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToPanel(3);
    }

    public void SwitchToPanel(int index)
    {
        if (_lerpToPanelRoutine != null) return;
        Vector3 target = new();
        switch(index)
        {
            case 0: target = _menuRotation;
                break;
            case 1: target = _generalRotation;
                break;
            case 2: target = _gameplayRotation;
                break;
            case 3: target = _keybindsRotation;
                break;
        }
        if (_mainCam.transform.rotation == Quaternion.Euler(target)) return;
        _lerpToPanelRoutine = StartCoroutine(LerpToPanel(target));
    }

    Coroutine _lerpToPanelRoutine;
    IEnumerator LerpToPanel(Vector3 targetRotation)
    {
        float lerpPos = 0;
        var targetRot = Quaternion.Euler(targetRotation);
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 0.75f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutCubic(lerpPos);
            _mainCam.transform.rotation = Quaternion.Lerp(_currentRotation, targetRot, t);
            yield return null;
        }
        _currentRotation = targetRot;
        _lerpToPanelRoutine = null;
    }
}