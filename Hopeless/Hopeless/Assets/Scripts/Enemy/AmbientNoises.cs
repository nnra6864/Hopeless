using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoises : MonoBehaviour
{
    [SerializeField] List<string> _noises;

    void Start() => StartCoroutine(PlayNoises());

    IEnumerator PlayNoises()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 20));
            SFX.PlaySFX(gameObject, _noises[Random.Range(0, _noises.Count)], Prefs.Instance.SpatialAudio, false, Random.Range(0.1f, 2f));
        }
    }
}