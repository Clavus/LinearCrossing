using UnityEngine;
using System.Collections;

public class Fade
{
    public static void FadeToAlpha(float alpha, float fadeSeconds)
    {
        FadeCanvasScript.instance.FadeToAlpha(alpha, fadeSeconds);
    }
}
