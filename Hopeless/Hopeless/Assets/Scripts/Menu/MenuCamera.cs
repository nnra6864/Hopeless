using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] Camera _mainCam;
    [SerializeField] Vector3 _menuRotation, _generalRotation, _gameplayRotation, _keybindsRotation, _customizationRotation;
    Quaternion _currentRotation = Quaternion.Euler(Vector3.zero);
    public static int SelectedCount;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(Prefs.KeyBinds[Prefs.Actions.Pause])) SwitchToPanel(0);
        if (Input.GetKeyUp(KeyCode.Alpha2)) SwitchToPanel(1);
        if (Input.GetKeyUp(KeyCode.Alpha3)) SwitchToPanel(2);
        if (Input.GetKeyUp(KeyCode.Alpha4)) SwitchToPanel(3);
        if (Input.GetKeyUp(KeyCode.Alpha5)) SwitchToPanel(4);
        if (Input.GetKeyUp(KeyCode.N)) SwitchToPanel(5);
    }

    public void SwitchToPanel(int index)
    {
        if (_lerpToPanelRoutine != null || SelectedCount > 0 || Keybind.IsListening) return;
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
            case 4: target = _customizationRotation;
                break;
            case 5: target = new(0, 90, 0);
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
        SFX.PlaySound("MenuCamera");
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