using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image ScreenImage;
    public Text ScreenText;

    private void Start()
    {
        ScreenText.canvasRenderer.SetAlpha(0.0f);
    }

    public void FadeInScreen(float fadeTime)
    {
        ScreenImage.CrossFadeAlpha(1f, fadeTime, false);
    }

    public void FadeInText(float fadeTime)
    {
        ScreenText.CrossFadeAlpha(1f, fadeTime, false);
    }

    public void FadeOutScreen(float fadeTime)
    {
        ScreenImage.CrossFadeAlpha(0f, fadeTime, false);
    }

    public void FadeOutText(float fadeTime)
    {
        ScreenText.CrossFadeAlpha(0f, fadeTime, false);
    }
}
