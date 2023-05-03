using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Noises : MonoBehaviour
{
    [SerializeField] AudioMixerGroup _mixerGroup;
    [SerializeField] List<AudioClip> _audioClips;
    [SerializeField] AudioSource _ambientSource;
    float _volume = 1;
    Vector2 _range = new(3, 15);
    void Start() => StartCoroutine(PlayNoises());

    IEnumerator PlayNoises()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_range.x, _range.y));
            StartCoroutine(PlayNoise(_audioClips[Random.Range(0, _audioClips.Count)]));
        }
    }

    IEnumerator PlayNoise(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.panStereo = Random.Range(0.0f, 1.0f);
        source.outputAudioMixerGroup = _mixerGroup;
        source.volume = _volume;
        source.Play();
        yield return new WaitForSeconds(clip.length + 0.5f);
        Destroy(source);
    }

    public void LowerNoises(float amount)
    {
        _volume -= amount;
        if (_lerpAmbientVolume != null)
        {
            StopCoroutine(_lerpAmbientVolume);
            _lerpAmbientVolume = null;
        }
        _lerpAmbientVolume = StartCoroutine(LerpAmbientVolume());
        _range.x = Mathf.Lerp(5, 3, _volume);
        _range.y = Mathf.Lerp(25, 15, _volume);
    }

    Coroutine _lerpAmbientVolume;
    IEnumerator LerpAmbientVolume()
    {
        var startingVolume = _ambientSource.volume;
        float lerpPos = 0;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime;
            lerpPos = Mathf.Clamp01(lerpPos);
            _ambientSource.volume = Mathf.Lerp(startingVolume, _volume, lerpPos);
            yield return null;
        }
        _lerpAmbientVolume = null;
    }
}