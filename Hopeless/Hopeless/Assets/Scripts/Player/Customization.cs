using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class Customization : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] TrailRenderer _trail;
    [SerializeField] List<ParticleSystem> _playerParticles;
    [SerializeField] ParticleSystem _shootParticles;
    [SerializeField] List<Light2D> _lights;
    [SerializeField] Material _bull;
    [SerializeField] LineRenderer _trajectory;
    [SerializeField] VisualEffect _deathEffect;

    private void Awake()
    {
        var playerColor = NnUtils.HexToRgba(PlayerPrefs.GetString("PlayerColor", "#FFFFFF"), Color.white);
        var particlesGradient = new Gradient();
        var mat = _player.GetComponent<Renderer>().material;
        mat.color = playerColor;

        _trail.startColor = playerColor;
        _trail.endColor = playerColor;
        _trail.material.color = playerColor;

        particlesGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(playerColor, 0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f) }
        );
        foreach (var particle in _playerParticles)
        {
            var p = particle.main;
            p.startColor = particlesGradient;
        }
        var bp = _shootParticles.main;
        bp.startColor = particlesGradient;
        foreach (var light in _lights)
        {
            light.color = playerColor;
        }

        _trajectory.startColor = playerColor;
        _trajectory.endColor = playerColor;
        _deathEffect.SetVector4("Color", (Vector4)(Color)playerColor * 2);
    }
}