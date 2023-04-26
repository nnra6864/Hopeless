using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TogglePauseMenu _togglePauseMenu;
    [SerializeField] Player.Player _playerScript;

    public void Resume() => _togglePauseMenu.Toggle();
    public void Respawn() { if (_togglePauseMenu.Toggle()) _playerScript.Die(); }
    public void Exit() { Time.timeScale = 1; SceneManager.LoadScene(0); }
}