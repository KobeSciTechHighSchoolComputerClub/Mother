using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMeshProUGUI;
    public float BlinkSpeed = 1f;

    void Start()
    {
        textMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, Mathf.Abs(Mathf.Sin(Time.time * BlinkSpeed)));
    }
}
