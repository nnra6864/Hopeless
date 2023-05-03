using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] TogglePauseMenu _pauseMenu;
    [SerializeField] Player.Player _player;
    [SerializeField] GameObject _circle;
    [SerializeField] RectTransform _credits;
    [SerializeField] TMP_Text _creditsText;
    [SerializeField] RectTransform _sanity;

    public void Execute()
    {
        TogglePauseMenu.HasEnded = true;
        if (TogglePauseMenu.IsActive) _pauseMenu.ForceClose();
        var pCol = NnUtils.HexToRgba(PlayerPrefs.GetString("PlayerColor", "#FFFFFFFF"), new Color32(255, 255, 255, 255));
        _circle.GetComponent<SpriteRenderer>().color = pCol;
        _creditsText.color = (Color)pCol * 5;
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(5);
        _player.End();
        _player.ToggleEndComponents();
        float lerpPos = 0;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 0.5f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutBack(lerpPos);
            _sanity.anchoredPosition = Vector2.Lerp(new Vector2(50, -50), new Vector2(-50, 50), lerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(3.6f);
        _circle.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        lerpPos = 0;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 1f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutQuad(lerpPos);
            _circle.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        lerpPos = 0;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 1f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOutQuad(lerpPos);
            _circle.transform.localScale = Vector3.Lerp(Vector3.one * 5, Vector3.one * 50, t);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        var rend = _circle.GetComponent<SpriteRenderer>();
        lerpPos = 0;
        var startingCol = rend.color;
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 3f;
            lerpPos = Mathf.Clamp01(lerpPos);
            var t = NnUtils.EaseInOut(lerpPos);
            rend.color = Vector4.Lerp(startingCol, Color.black, t);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        lerpPos = 0;
        Vector2 startingCredits = new(0, -1000);
        Vector2 endingCredits = new(0, 1000);
        while (lerpPos < 1)
        {
            lerpPos += Time.deltaTime / 20f;
            lerpPos = Mathf.Clamp01(lerpPos);
            _credits.anchoredPosition = Vector2.Lerp(startingCredits, endingCredits, lerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}