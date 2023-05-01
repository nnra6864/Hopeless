using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] Prefs.Actions _primaryAction, _secondaryAction;
    [SerializeField] TMP_Text _text;

    private void Start()
    {
        _text.text = _text.text.Replace("{1}", Prefs.KeyBinds[_primaryAction].ToString()).Replace("{2}", Prefs.KeyBinds[_secondaryAction].ToString());
    }
}