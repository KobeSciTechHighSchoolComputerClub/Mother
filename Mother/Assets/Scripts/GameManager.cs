using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private bool[] line;
    public ChangeSiteImage changeSiteImage;
    void Start()
    {
        line = new bool[4];
        for (int i = 0; i < line.Length; ++i)
        {
            line[i] = false;
        }
        gameManager = this.GetComponent<GameManager>();
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
