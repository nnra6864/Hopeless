using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    public enum Actions
    {
        MoveLeft, MoveLeftSecondary,
        MoveRight, MoveRightSecondary,
        Jump, JumpSecondary,
        Ground, GroundSecondary,
        Dash, DashSecondary,
        Shoot, ShootSecondary,
        Pause
    }

    private static Prefs _instance;
    public static Prefs Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        UpdateValues();
        LoadKeybinds();
    }

    void UpdateValues()
    {
        UseDeathEffect = PlayerPrefs.GetInt("DeathEffect", 1) == 1;
        UseMemoryEffect = PlayerPrefs.GetInt("MemoryEffect", 1) == 1;
        TrajectorySize = PlayerPrefs.GetFloat("TrajectorySize", 0.1f);
        SpatialAudio = PlayerPrefs.GetInt("SpatialAudio", 1);
    }

    public delegate void OnValueChanged();

    private bool _useDeathEffect;
    public bool UseDeathEffect
    {
        get => _useDeathEffect;
        set
        {
            _useDeathEffect = value;
            PlayerPrefs.SetInt("DeathEffect", value ? 1 : 0);
        }
    }

    private bool _useMemoryEffect;
    public bool UseMemoryEffect
    {
        get => _useMemoryEffect;
        set
        {
            _useMemoryEffect = value;
            PlayerPrefs.SetInt("MemoryEffect", value ? 1 : 0);
        }
    }

    public event OnValueChanged OnTrajectorySizeChanged;
    private float _trajectorySize;
    public float TrajectorySize
    {
        get => _trajectorySize;
        set
        {
            _trajectorySize = value;
            PlayerPrefs.SetFloat("TrajectorySize", value);
            OnTrajectorySizeChanged?.Invoke();
        }
    }

    private int _spatialAudio;
    public int SpatialAudio
    {
        get => _spatialAudio;
        set
        {
            _spatialAudio = value;
            PlayerPrefs.SetInt("SpatialAudio", value);
        }
    }

    public static Dictionary<Actions, KeyCode> KeyBinds = new();

    public static void Reset()
    {
        KeyBinds = new()
        {
            { Actions.MoveLeft, KeyCode.A },
            { Actions.MoveLeftSecondary, KeyCode.LeftArrow },
            { Actions.MoveRight, KeyCode.D },
            { Actions.MoveRightSecondary, KeyCode.RightArrow },
            { Actions.Jump, KeyCode.Space },
            { Actions.JumpSecondary, KeyCode.W },
            { Actions.Ground, KeyCode.LeftControl },
            { Actions.GroundSecondary, KeyCode.S },
            { Actions.Dash, KeyCode.LeftShift },
            { Actions.DashSecondary, KeyCode.F },
            { Actions.Shoot, KeyCode.Mouse0 },
            { Actions.ShootSecondary, KeyCode.Return },
            { Actions.Pause, KeyCode.Escape }
        };
    }

    static void LoadKeybinds()
    {
        Reset();
        foreach (Actions action in Enum.GetValues(typeof(Actions)))
        {
            var key = PlayerPrefs.GetString(action+"Key", string.Empty);
            if (key == string.Empty) continue;
            KeyBinds[action] = (KeyCode)Enum.Parse(typeof(KeyCode), key);
        }
    }

    public static void BindKey(Actions action, KeyCode keyCode)
    {
        KeyBinds[action] = keyCode;
        PlayerPrefs.SetString(action + "Key", keyCode.ToString());
    }
}