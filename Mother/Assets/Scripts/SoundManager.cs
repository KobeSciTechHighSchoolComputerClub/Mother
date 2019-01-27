using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void FadeIn()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(fadeIn(10f));
    }

    public void FadeOut()
    {
        StartCoroutine(fadeOut(1f));
    }

    IEnumerator fadeIn(float fadeTime)
    {
        float t = Time.time;
        while (true)
        {
            float tt = Time.time - t;
            if(tt > fadeTime)
            {
                break;
            }
            GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, tt / fadeTime);
            yield return null;
        }
    }

    IEnumerator fadeOut(float fadeTime)
    {
        float t = Time.time;
        while (true)
        {
            float tt = Time.time - t;
            if (tt > fadeTime)
            {
                GetComponent<AudioSource>().Stop();
                break;
            }
            GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, tt / fadeTime);
            yield return null;
        }
    }
}
