using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] Prefs.Actions _primaryAction, _secondaryAction;
    [SerializeField] TMP_Text _text;

    private void Start()
    {
        _text.text = $"{Prefs.KeyBinds[_primaryAction]}/{Prefs.KeyBinds[_secondaryAction]} - {_text.text}";
    }
}