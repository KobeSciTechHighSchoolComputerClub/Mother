using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGE : MonoBehaviour
{
    public float BabySpeed;
    public GameObject HandStamp;
    public Transform[] HandPoints;
    private int handCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeHandStamps());
    }
    private IEnumerator makeHandStamps()
    {
        while (true)
        {
            Transform tra = HandPoints[handCount = (handCount + 1) % HandPoints.Length];
            GameObject d = Instantiate(HandStamp) as GameObject;
            d.transform.position = tra.position;
            d.transform.rotation = Quaternion.LookRotation(Vector3.down, getForward());
            d.transform.localScale = tra.localScale * d.transform.localScale.x;
            Destroy(d, 10f);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private Vector3 getForward()
    {
        Vector3 res = this.transform.forward;
        res.y = 0;
        return res.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        babymove();
    }

    private void lookobject()
    {

    }

    private void babymove()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float lv = Input.GetAxis("R_Stick_V");
        float lh = Input.GetAxis("R_Stick_H");
        this.transform.Rotate(0f, hor + lh, 0f, Space.World);
        Camera.main.transform.Rotate(lv, 0f, 0f, Space.Self);
        Vector3 fo = getForward();
        this.transform.position += fo * BabySpeed * Time.deltaTime * ver;
    }
}
