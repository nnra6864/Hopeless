using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] RectTransform _uiTransofrm;
    [SerializeField] TMP_Text _rewardText;
    [SerializeField] Vector2 _hiddenPosition, _shownPosition;
    Dictionary<string, float> _rewards = new();

    public void DisplayReward(string text, float time = 3)
    {
        if (_displayRewardRoutine != null)
        {
            _rewards.Add(text, time);
            return;
        }
        _displayRewardRoutine = StartCoroutine(DisplayRewardRoutine(text, time));
    }

    Coroutine _displayRewardRoutine;
    public IEnumerator DisplayRewardRoutine(string message, float time)
    {
        _rewards.Remove(message);
        _rewardText.text = message;
        float lerpPos = 0;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 1f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutCubic(lerpPos);
            _uiTransofrm.anchoredPosition = Vector2.Lerp(_hiddenPosition, _shownPosition, t);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        while (lerpPos > 0)
        {
            lerpPos -= Time.deltaTime / 1f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutCubic(lerpPos);
            _uiTransofrm.anchoredPosition = Vector2.Lerp(_hiddenPosition, _shownPosition, t);
            yield return null;
        }
        _displayRewardRoutine = null;
        if (_rewards.Count < 1) yield break;
        _displayRewardRoutine = StartCoroutine(DisplayRewardRoutine(_rewards.ElementAt(0).Key, _rewards.ElementAt(0).Value));
    }
}