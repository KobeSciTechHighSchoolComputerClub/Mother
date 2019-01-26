using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownSensor : MonoBehaviour
{
    public bool isFlying = false;
    private void OnTriggerExit(Collider other)
    {
        isFlying = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        isFlying = false;
    }
}
