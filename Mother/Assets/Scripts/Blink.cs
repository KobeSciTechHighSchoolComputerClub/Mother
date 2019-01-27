using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMeshProUGUI;
    public float BlinkSpeed = 1f;
    bool onFading;

    void Start()
    {
        textMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
        onFading = false;
    }

    void Update()
    {
        if(!onFading)
            textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, Mathf.Abs(Mathf.Sin(Time.time * BlinkSpeed)));
    }
    
    public void FadeOut(float fadeTime)
    {
        StartCoroutine(fadeOut(fadeTime));
    }

    IEnumerator fadeOut(float fadeTime)
    {
        onFading = true;
        float t = Time.time;
        float a = textMeshProUGUI.color.a;
        while (true)
        {
            float tt = Time.time - t;
            if (tt > fadeTime)
            {
                break;
            }
            textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, Mathf.Lerp(a, 0f, tt / fadeTime));
            yield return null;
        }
    }
}
