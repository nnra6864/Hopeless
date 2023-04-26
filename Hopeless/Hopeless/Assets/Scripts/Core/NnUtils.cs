using System.Globalization;
using UnityEngine;

public class NnUtils
{
    #region Easing Functions
    #region Sine
    public static float EaseIn(float t) => 1 - Mathf.Cos(t * Mathf.PI / 2f);
    public static float EaseOut(float t) => Mathf.Sin(t * Mathf.PI / 2f);
    public static float EaseInOut(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
    public static float EaseOutIn(float t) => t < 0.5f ? EaseOut(2 * t) / 2 : 0.5f + EaseIn(2 * t - 1) / 2;
    public static float EaseInBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return c3 * t * t * t - c1 * t * t;
    }
    #endregion
    #region Quad
    public static float EaseInQuad(float t) => t * t;
    public static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);
    public static float EaseInOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    #endregion
    #region Cubic
    public static float EaseInCubic(float t) => Mathf.Pow(t, 3f);
    public static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    public static float EaseInOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    public static float EaseOutInCubic(float t) => t < 0.5f ? EaseOutCubic(t * 2) / 2 : 0.5f + EaseInCubic(t * 2 - 1) / 2;
    #endregion
    #region Quint
    public static float EaseInOutQuint(float t) => t < 0.5f ? 16 * Mathf.Pow(t, 5) : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
    public static float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
    public static float EaseInCubicOutQuint(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    #endregion
    #region Circ
    public static float EaseInCirc(float t) => 1f - Mathf.Sqrt(1f - t * t);
    public static float EaseOutCirc(float t) => Mathf.Sqrt(1f - Mathf.Pow(1f - t, 2f));
    public static float EaseInOutCirc(float t) => t < 0.5f ? (1f - Mathf.Sqrt(1f - 4f * t * t)) / 2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
    public static float EaseOutInCirc(float t) => t < 0.5f ? EaseOutCirc(2f * t) / 2f : (EaseInCirc(2f * t - 1f) + 1f) / 2f;
    #endregion
    #endregion
    public static Color32 HexToRgba(string hex, Color32 currentColor)
    {
        if (hex.Length < 1) return currentColor;
        int i = hex[0] == '#' ? 1 : 0;
        int r = currentColor.r, g = currentColor.g, b = currentColor.b, a = currentColor.a;
        if (hex.Length >= 2 + i) if (!int.TryParse($"{hex[0 + i]}{hex[1 + i]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out r)) { }
        if (hex.Length >= 4 + i) if (!int.TryParse($"{hex[2 + i]}{hex[3 + i]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g)) { }
        if (hex.Length >= 6 + i) if (!int.TryParse($"{hex[4 + i]}{hex[5 + i]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b)) { }
        if (hex.Length >= 8 + i) if (!int.TryParse($"{hex[6 + i]}{hex[7 + i]}", NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a)) { }
        return new Color32(byte.Parse(r.ToString()), byte.Parse(g.ToString()), byte.Parse(b.ToString()), byte.Parse(a.ToString()));
    }
}