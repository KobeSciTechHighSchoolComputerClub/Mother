using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAction : MonoBehaviour
{
    public LayerMask Mask;
    public float Range;
    public float CatchPower;
    private GameObject CatchPoint;
    private float catchLength;
    // Start is called before the first frame update
    void Start()
    {
        CatchPoint = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(CatchPoint != null)
            {
                Destroy(CatchPoint);
                return;
            }

            RaycastHit hit = new RaycastHit();
            if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Range, Mask)) return;
            if (hit.collider.tag != "CanMove") return;
            GameObject d = new GameObject();
            d.transform.position = hit.point;
            d.transform.parent = hit.collider.transform;
            catchLength = hit.distance;
        }

        if(CatchPoint != null)
        {
            Vector3 point = this.transform.position + this.transform.forward * catchLength;
            point = (point - CatchPoint.transform.position).normalized * CatchPower;
        }
    }
}
