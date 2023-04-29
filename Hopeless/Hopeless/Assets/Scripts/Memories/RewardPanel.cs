using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] RectTransform _uiTransofrm;
    [SerializeField] TMP_Text _rewardText;
    [SerializeField] Vector2 _hiddenPosition, _shownPosition;
    Queue<string> _rewards = new();

    public void DisplayReward(string text)
    {
        if (_displayRewardRoutine != null)
        {
            _rewards.Enqueue(text);
            return;
        }
        _displayRewardRoutine = StartCoroutine(DisplayRewardRoutine(text));
    }

    Coroutine _displayRewardRoutine;
    public IEnumerator DisplayRewardRoutine(string message)
    {
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
        yield return new WaitForSeconds(3);
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
        _displayRewardRoutine = StartCoroutine(DisplayRewardRoutine(_rewards.Dequeue()));
    }
}