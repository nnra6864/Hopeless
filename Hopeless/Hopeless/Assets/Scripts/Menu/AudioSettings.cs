using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _ambient, _sfx;
    private void Start()
    {
        var ambient = PlayerPrefs.GetFloat("AmbientVolume", -40);
        var sfx = PlayerPrefs.GetFloat("SFXVolume", 0);
        _audioMixer.SetFloat("Ambient", ambient);
        _audioMixer.SetFloat("SFX", sfx);
        _ambient.SetValueWithoutNotify(ambient);
        _sfx.SetValueWithoutNotify(sfx);
    }

    public void ChangeAmbientVolume(float value)
    {
        PlayerPrefs.SetFloat("AmbientVolume", value);
        _audioMixer.SetFloat("Ambient", value);
    }

    public void ChangeSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        _audioMixer.SetFloat("SFX", value);
    }
}