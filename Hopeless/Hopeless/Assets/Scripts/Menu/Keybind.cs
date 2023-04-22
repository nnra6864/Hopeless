using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Keybind : MonoBehaviour
{
    [SerializeField] Prefs.Actions _action;
    [SerializeField] TMP_Text _buttonText;
    private KeyCode _key;
    public static bool IsListening = false;

    private void Start()
    {
        _buttonText.text = Prefs.KeyBinds[_action].ToString();
    }

    public void BindKey()
    {
        if (IsListening) return;
        if (_bindKeyRoutine != null)
        {
            StopCoroutine(_bindKeyRoutine);
            _bindKeyRoutine = null;
        }
        _bindKeyRoutine = StartCoroutine(BindKeyRoutine());
    }

    Coroutine _bindKeyRoutine;
    IEnumerator BindKeyRoutine()
    {
        IsListening = true;
        _buttonText.text = "Press Any Key";
        KeyCode[] allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        yield return null;
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _bindKeyRoutine = null;
                _buttonText.text = _key.ToString();
                IsListening = false;
                yield break;
            }
            foreach (KeyCode key in allKeyCodes)
            {
                if (!Input.GetKeyUp(key)) continue;
                Prefs.BindKey(_action, key);
                _key = key;
                _buttonText.text = key.ToString();
                IsListening = false;
                yield break;
            }
            yield return null;
        }
    }
}