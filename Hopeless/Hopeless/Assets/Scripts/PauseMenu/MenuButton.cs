using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] Image _targetImage, _otherImage;
    [SerializeField] MenuButton _otherButton;
    [SerializeField] RectTransform _panel;
    Vector2 _menuPosition = new(250, -15), _settingsPosition = new(-250, -15);
    Color32 _normalColor = new(0, 0, 0, 125), _hoverColor = new(0, 0, 0, 150), _mouseDownColor = new(0, 0, 0, 175), _selectedColor = new(0, 0, 0, 200);

    public void OnPointerEnter()
    {
        if ((int)(_targetImage.color.a * 255) > 150) return;
        LerpColor(_hoverColor);
    }

    public void OnPointerExit()
    {
        if ((int)(_targetImage.color.a * 255) > 150) return;
        LerpColor(_normalColor);
    }

    public void OnPointerDown()
    {
        if ((int)(_targetImage.color.a * 255) > 175) return;
        LerpColor(_mouseDownColor);
    }

    public void OnPointerUp()
    {
        if ((int)(_targetImage.color.a * 255) > 175) return;
        LerpColor(_hoverColor);
    }

    public void OnPointerClick(int dir)
    {
        LerpColor(_selectedColor);
        _otherButton.LerpColor(_normalColor);
        LerpMenu(dir);
    }

    public void LerpColor(Color32 targetColor)
    {
        if (_lerpColorRoutine != null)
        {
            StopCoroutine(_lerpColorRoutine);
            _lerpColorRoutine = null;
        }
        _lerpColorRoutine = StartCoroutine(LerpColorRoutine(targetColor));
    }

    Coroutine _lerpColorRoutine;
    IEnumerator LerpColorRoutine(Color32 targetColor)
    {
        float lerpPos = 0;
        Color32 _startingColor = _targetImage.color;
        while (lerpPos < 1)
        {
            lerpPos += Time.unscaledDeltaTime / 0.1f;
            lerpPos = Mathf.Clamp01(lerpPos);
            float t = NnUtils.EaseInOut(lerpPos);
            _targetImage.color = Vector4.Lerp((Color)_startingColor, (Color)targetColor, t);
            yield return null;
        }
        _lerpColorRoutine = null;
    }

    void LerpMenu(int dir)
    {
        if (_lerpMenuRoutine != null)
        {
            StopCoroutine(_lerpMenuRoutine);
            _lerpMenuRoutine = null;
        }
        _lerpMenuRoutine = StartCoroutine(LerpMenuRoutine(dir));
    }

    static float _menuLerpPos = 0;
    static Coroutine _lerpMenuRoutine;
    IEnumerator LerpMenuRoutine(int dir)
    {
        Vector2 startingPos = _panel.anchoredPosition;
        while (dir == 1 ? _menuLerpPos < 1 : _menuLerpPos > 0)
        {
            _menuLerpPos += (Time.unscaledDeltaTime * dir) / 0.5f;
            _menuLerpPos = Mathf.Clamp01(_menuLerpPos);
            var t = NnUtils.EaseInOutQuad(_menuLerpPos);
            _panel.anchoredPosition = Vector2.Lerp(_menuPosition, _settingsPosition, t);
            yield return null;
        }
        _lerpMenuRoutine = null;
    }
}