using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] Toggle _deathEffectToggle, _memoryEffectToggle;
    [SerializeField] Slider _trajectorySizeSlider;

    private void Start()
    {
        _deathEffectToggle.SetIsOnWithoutNotify(Prefs.Instance.UseDeathEffect);
        _memoryEffectToggle.SetIsOnWithoutNotify(Prefs.Instance.UseMemoryEffect);
        _trajectorySizeSlider.SetValueWithoutNotify(Prefs.Instance.TrajectorySize);
    }

    public void SetDeathEffect(bool value) => Prefs.Instance.UseDeathEffect = value;
    public void SetMemoryEffect(bool value) => Prefs.Instance.UseMemoryEffect = value;
    public void SetTrajectorySize(float value) => Prefs.Instance.TrajectorySize = value;
}