using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
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
    }

    void UpdateValues()
    {
        UseDeathEffect = PlayerPrefs.GetInt("DeathEffect", 1) == 1;
        UseMemoryEffect = PlayerPrefs.GetInt("MemoryEffect", 1) == 1;
        TrajectorySize = PlayerPrefs.GetFloat("TrajectorySize", 0.1f);
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
}