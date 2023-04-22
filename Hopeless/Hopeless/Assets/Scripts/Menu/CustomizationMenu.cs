using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationMenu : MonoBehaviour
{
    public void StartedEditing() => MenuCamera.SelectedCount++;
    public void FinishedEditing() => MenuCamera.SelectedCount--;

    public void SetPlayerColor(string input)
    {
        PlayerPrefs.SetString("PlayerColor", input);
    }
}