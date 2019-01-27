using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public SoundFader[] BGMs;
    public SceneObject FirstStage;
    public Blink textBlink; 
    void Start()
    {
        BGMs[0].FadeIn(5f);
        BGMs[1].FadeIn(5f);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Invoke("loadFirstScene", 3f);
            BGMs[0].FadeOut(2.5f);
            BGMs[1].FadeOut(2.5f);
            textBlink.FadeOut(2.5f);
        }
    }

    void loadFirstScene()
    {
        SceneManager.LoadScene(FirstStage);
    }

    void whiteOut()
    {

    }
}
