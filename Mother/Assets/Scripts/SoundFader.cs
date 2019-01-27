using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFader : MonoBehaviour
{
    public float MaxVolume = 1f;

    public void FadeIn(float fadeTime)
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(fadeIn(fadeTime));
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(fadeOut(fadeTime));
    }

    IEnumerator fadeIn(float fadeTime)
    {
        float t = Time.time;
        GetComponent<AudioSource>().volume = 0f;
        while (true)
        {
            float tt = Time.time - t;
            if(tt > fadeTime)
            {
                break;
            }
            GetComponent<AudioSource>().volume = Mathf.Lerp(0f, MaxVolume, tt / fadeTime);
            yield return null;
        }
    }

    IEnumerator fadeOut(float fadeTime)
    {
        float t = Time.time;
        float v = GetComponent<AudioSource>().volume;
        while (true)
        {
            float tt = Time.time - t;
            if (tt > fadeTime)
            {
                GetComponent<AudioSource>().Stop();
                break;
            }
            GetComponent<AudioSource>().volume = Mathf.Lerp(v, 0f, tt / fadeTime);
            yield return null;
        }
    }
}
