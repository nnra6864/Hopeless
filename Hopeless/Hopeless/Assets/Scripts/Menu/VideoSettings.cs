using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionsDropdown, _fullscreenModeDropdown;
    List<Resolution> _resolutions = new();

    private void Start()
    {
        _fullscreenModeDropdown.SetValueWithoutNotify(
            Screen.fullScreenMode == FullScreenMode.Windowed
            ? 0 : Screen.fullScreenMode == FullScreenMode.FullScreenWindow
            ? 1 : 2);
        _resolutions.Clear();
        _resolutionsDropdown.ClearOptions();
        UpdateResolutions();
        _resolutionsDropdown.AddOptions(_resolutions.Select(res => $"{res.width}x{res.height}").ToList());
        _resolutionsDropdown.SetValueWithoutNotify(_resolutions.IndexOf(Screen.currentResolution));
    }

    void UpdateResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        _resolutions = resolutions.OrderByDescending(res => res.refreshRateRatio)
                                   .GroupBy(res => new Vector2Int(res.width, res.height))
                                   .Select(group => group.First())
                                   .ToList();
    }

    public void SetResolution(int index) => Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreenMode);
    public void SetFullScreenMode(int index) => Screen.fullScreenMode = index == 0 ? FullScreenMode.Windowed : index == 1 ? FullScreenMode.FullScreenWindow : FullScreenMode.ExclusiveFullScreen;
}
