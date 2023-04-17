using Unity.VisualScripting;
using UnityEngine;

public class NnUtils
{
    public static float EaseIn(float t) => 1 - Mathf.Cos((t * Mathf.PI) / 2f);
    public static float EaseOut(float t) => Mathf.Sin((t * Mathf.PI) / 2f);
    public static float EaseInOut(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
    public static float EaseInOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    public static float EaseInOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    public static float EaseInBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return c3 * t * t * t - c1 * t * t;
    }
    public static float EaseInOutQuint(float t) => t < 0.5f ? 16 * Mathf.Pow(t, 5) : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
    public static float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
    public static float EaseInCubicOutQuint(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
}