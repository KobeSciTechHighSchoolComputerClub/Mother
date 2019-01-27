using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public SoundManager soundManager;
    void Start()
    {
        soundManager.FadeIn();
    }
}
