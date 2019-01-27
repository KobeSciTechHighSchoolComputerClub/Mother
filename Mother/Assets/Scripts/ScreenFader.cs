using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image ScreenImage;

    public void FadeIn(float fadeTime)
    {
        ScreenImage.CrossFadeAlpha(0f, fadeTime, false);
    }

    public void FadeOut(float fadeTime)
    {
        ScreenImage.CrossFadeAlpha(1f, fadeTime, false);
    }
}
