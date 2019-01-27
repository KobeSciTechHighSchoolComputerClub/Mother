﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private bool[] line;
    public ChangeSiteImage changeSiteImage;
    public ScreenFader screenFader;
    public AudioSource BGM, SE;
    public AudioClip BGMclip, CallMom, Gatcha;
    void Start()
    {
        line = new bool[4];
        for (int i = 0; i < line.Length; ++i)
        {
            line[i] = false;
        }
        gameManager = this.GetComponent<GameManager>();
        intro();
    }

    public void GatchaSound()
    {
        SE.clip = Gatcha;
        SE.Play();
    }

    void intro()
    {
        SE.clip = CallMom;
        SE.Play();
        screenFader.FadeInText(1f);
        Invoke("screenFadeOut", 5f);
    }

    void screenFadeOut()
    {
        screenFader.FadeOutScreen(2f);
        screenFader.FadeOutText(1f);
        BGM.clip = BGMclip;
        BGM.Play();

    }

    public void SetClimb(bool flag)
    {
        line[3] = flag;
        setImage();
    }
    public void SetHand(bool flag)
    {
        line[1] = flag;
        setImage();
    }
    public void SetGrub(bool flag)
    {
        line[2] = flag;
        setImage();
    }
    private void setImage()
    {
        if (line[2])
        {
            changeSiteImage.Grub();
        }
        else
        if (line[1])
        {
            changeSiteImage.Hand();
        }
        else
        if (line[3])
        {
            changeSiteImage.Climb();
        }
        else
        {
            changeSiteImage.Default();
        }
    }
}
