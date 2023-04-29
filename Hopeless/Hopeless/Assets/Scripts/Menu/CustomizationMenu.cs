using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField _hexInput;
    [SerializeField] Slider _rInput, _gInput, _bInput;
    [SerializeField] Image _preview;
    [SerializeField] string _target;
    Color32 _currentColor;

    private void Start()
    {
        _hexInput.SetTextWithoutNotify(PlayerPrefs.GetString(_target, new Color32(255, 255, 255, 255).ToString()));
        _currentColor = NnUtils.HexToRgba(_hexInput.text, new Color32(255, 255, 255, 255));
        UpdateUI(-1);
    }

    public void StartedEditing() => MenuCamera.SelectedCount++;
    public void FinishedEditing() => MenuCamera.SelectedCount--;

    public void SetPlayerColor(string input)
    {
        PlayerPrefs.SetString("PlayerColor", input);
    }

    public void ChangeHex(string input)
    {
        var col = NnUtils.HexToRgba(input, _currentColor);
        _currentColor = new(col.r, col.g, col.b, 255);
        UpdateUI(0);
        PlayerPrefs.SetString(_target, _hexInput.text);
    }

    public void ChangeR(float input)
    {
        _currentColor = new Color32((byte)input, _currentColor.g, _currentColor.b, _currentColor.a);
        UpdateUI(1);
        PlayerPrefs.SetString(_target, _hexInput.text);
    }

    public void ChangeG(float input)
    {
        _currentColor = new Color32(_currentColor.r, (byte)input, _currentColor.b, _currentColor.a);
        UpdateUI(2);
        PlayerPrefs.SetString(_target, _hexInput.text);
    }

    public void ChangeB(float input)
    {
        _currentColor = new Color32(_currentColor.r, _currentColor.g, (byte)input, _currentColor.a);
        UpdateUI(3);
        PlayerPrefs.SetString(_target, _hexInput.text);
    }

    public void UpdateUI(int sender)
    {
        if (sender != 0) _hexInput.SetTextWithoutNotify("#"+ColorUtility.ToHtmlStringRGB(_currentColor));
        if (sender != 1) _rInput.SetValueWithoutNotify(_currentColor.r);
        if (sender != 2) _gInput.SetValueWithoutNotify(_currentColor.g);
        if (sender != 3) _bInput.SetValueWithoutNotify(_currentColor.b);
        if (_transitionRoutine != null)
        {
            StopCoroutine(_transitionRoutine);
            _transitionRoutine = null;
        }
        _transitionRoutine = StartCoroutine(TransitionRoutine());
    }

    Coroutine _transitionRoutine;
    IEnumerator TransitionRoutine()
    {
        float lerpPos = 0;
        Color32 _startingColor = _preview.color;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime;
            lerpPos = Mathf.Clamp01(lerpPos);
            float t = NnUtils.EaseInOutQuad(lerpPos);
            _preview.color = Vector4.Lerp((Color)_startingColor, (Color)_currentColor, t);
            yield return null;
        }
        _transitionRoutine = null;
    }
}