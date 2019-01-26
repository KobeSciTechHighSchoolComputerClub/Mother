using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownSensor : MonoBehaviour
{
    BabyMover babyMover;
    public bool isFront;
    private void Start()
    {
        babyMover = transform.parent.GetComponent<BabyMover>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isFront)
        {
            babyMover.TurnBack();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isFront)
        {
            babyMover.TurnBack();
        }
    }
}
