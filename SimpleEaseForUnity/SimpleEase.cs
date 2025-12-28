using System;
using UnityEngine;

// Original source from https://gist.github.com/Kryzarel/bba64622057f21a1d6d44879f9cd7bd4
public static class SimpleEase
{
    public static float Lerp(float v0, float v1, float t, Func<float, float> func)
    {
        return v0 + (v1 - v0) * func(t);
    }

    public static float InQuad(float t)
    {
        return t * t;
    }

    public static float OutQuad(float t)
    {
        return 1.0f - InQuad(1.0f - t);
    }

    public static float InOutQuad(float t)
    {
        if (t < 0.5f)
            return InQuad(t * 2.0f) / 2.0f;

        return 1.0f - InQuad((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InCubic(float t)
    {
        return t * t * t;
    }

    public static float OutCubic(float t)
    {
        return 1.0f - InCubic(1.0f - t);
    }

    public static float InOutCubic(float t)
    {
        if (t < 0.5f)
            return InCubic(t * 2.0f) / 2.0f;

        return 1.0f - InCubic((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InQuart(float t)
    {
        return t * t * t * t;
    }

    public static float OutQuart(float t)
    {
        return 1.0f - InQuart(1.0f - t);
    }

    public static float InOutQuart(float t)
    {
        if (t < 0.5f)
            return InQuart(t * 2.0f) / 2.0f;

        return 1.0f - InQuart((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InQuint(float t)
    {
        return t * t * t * t * t;
    }

    public static float OutQuint(float t)
    {
        return 1.0f - InQuint(1.0f - t);
    }

    public static float InOutQuint(float t)
    {
        if (t < 0.5f)
            return InQuint(t * 2.0f) / 2.0f;

        return 1.0f - InQuint((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InSine(float t)
    {
        return 1.0f - Mathf.Cos(t * Mathf.PI / 2.0f);
    }

    public static float OutSine(float t)
    {
        return Mathf.Sin(t * Mathf.PI / 2.0f);
    }

    public static float InOutSine(float t)
    {
        return (Mathf.Cos(t * Mathf.PI) - 1.0f) / -2.0f;
    }

    public static float InExpo(float t)
    {
        return Mathf.Pow(2.0f, 10.0f * (t - 1.0f));
    }

    public static float OutExpo(float t)
    {
        return 1.0f - InExpo(1.0f - t);
    }

    public static float InOutExpo(float t)
    {
        if (t < 0.5f)
            return InExpo(t * 2.0f) / 2.0f;

        return 1.0f - InExpo((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InCirc(float t)
    {
        return -(Mathf.Sqrt(1.0f - t * t) - 1.0f);
    }

    public static float OutCirc(float t)
    {
        return 1.0f - InCirc(1.0f - t);
    }

    public static float InOutCirc(float t)
    {
        if (t < 0.5f)
            return InCirc(t * 2.0f) / 2.0f;

        return 1.0f - InCirc((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InElastic(float t)
    {
        return 1.0f - OutElastic(1.0f - t);
    }

    public static float OutElastic(float t)
    {
        const float p = 0.3f;

        return Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t - p / 4.0f) * (2.0f * Mathf.PI) / p) + 1.0f;
    }

    public static float InOutElastic(float t)
    {
        if (t < 0.5f)
            return InElastic(t * 2.0f) / 2.0f;

        return 1.0f - InElastic((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InBack(float t)
    {
        const float s = 1.70158f;

        return t * t * ((s + 1.0f) * t - s);
    }

    public static float OutBack(float t)
    {
        return 1.0f - InBack(1.0f - t);
    }

    public static float InOutBack(float t)
    {
        if (t < 0.5f)
            return InBack(t * 2.0f) / 2.0f;

        return 1.0f - InBack((1.0f - t) * 2.0f) / 2.0f;
    }

    public static float InBounce(float t)
    {
        return 1.0f - OutBounce(1.0f - t);
    }

    public static float OutBounce(float t)
    {
        const float div = 2.75f;
        const float mult = 7.5625f;

        if (t < 1.0f / div)
        {
            return mult * t * t;
        }
        else if (t < 2.0f / div)
        {
            t -= 1.5f / div;

            return mult * t * t + 0.75f;
        }
        else if (t < 2.5f / div)
        {
            t -= 2.25f / div;

            return mult * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / div;

            return mult * t * t + 0.984375f;
        }
    }

    public static float InOutBounce(float t)
    {
        if (t < 0.5f)
            return InBounce(t * 2.0f) / 2.0f;

        return 1.0f - InBounce((1.0f - t) * 2.0f) / 2.0f;
    }
}
