using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSiteImage : MonoBehaviour
{
    public Sprite[] Images;

    public void Default()
    {
        this.GetComponent<Image>().sprite = Images[0];
    }

    public void Hand()
    {
        this.GetComponent<Image>().sprite = Images[1];
    }

    public void Grub()
    {
        this.GetComponent<Image>().sprite = Images[2];
    }

    public void Climb()
    {
        this.GetComponent<Image>().sprite = Images[3];
    }
}
