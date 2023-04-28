using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime : MonoBehaviour
{
    public delegate Action TimeChanged();
    public static Action OnTimeChanged;
    private static float _time;
    public static float Time
    {
        get => _time;
        set { if (_time == value) return;  _time = value; UnityEngine.Time.timeScale = value; OnTimeChanged?.Invoke(); }
    }
}