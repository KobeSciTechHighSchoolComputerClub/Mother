using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownSensor : MonoBehaviour
{
    BabyMover babyMover;
    public bool isFront;
    private int count;
    private void Start()
    {
        babyMover = transform.parent.GetComponent<BabyMover>();
        count = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isFront)
        {
            --count;
            if (count == 0)
            {
                babyMover.TurnBack();
            }
        }
        else
        {
            ++count;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isFront)
        {
            --count;
            if (count == 0)
            {
                babyMover.TurnBack();
            }
        }
        else
        {
            ++count;
        }
    }
}
