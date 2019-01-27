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
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Range, Mask);
        if (hit.collider != null && hit.collider.tag == "CanMove")
        {
            GameManager.gameManager.SetHand(true);
        }
        else
        {
            GameManager.gameManager.SetHand(false);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (hit.collider == null) return;
            if (hit.collider.tag == "CanMove")
            {
                GameObject d = new GameObject();
                d.transform.position = hit.point;
                d.transform.parent = hit.collider.transform;
                catchLength = hit.distance;
                CatchPoint = d;
                GameManager.gameManager.SetGrub(true);
            }else if(hit.collider.tag == "DoorKnob")
            {
                hit.collider.GetComponent<Animator>().SetBool("open", true);
                GameManager.gameManager.GatchaSound();
                GameManager.gameManager.screenFadeIn();
                GameManager.gameManager.LoadNextScene(3f);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (CatchPoint != null)
            {
                Destroy(CatchPoint);
                GameManager.gameManager.SetGrub(false);
                return;
            }
        }
        if(CatchPoint != null)
        {
            Vector3 point = this.transform.position + this.transform.forward * catchLength;
            point = (point - CatchPoint.transform.position).normalized;
            point = point.normalized * Mathf.Min(point.magnitude, 2f) * CatchPower;
            CatchPoint.transform.parent.GetComponent<Rigidbody>().AddForce(point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            GameManager.gameManager.screenFadeIn();
            GameManager.gameManager.LoadNextScene(3f);
        }
    }
}
