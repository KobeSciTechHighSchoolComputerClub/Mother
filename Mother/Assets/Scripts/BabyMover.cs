using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyMover : MonoBehaviour
{
    public float BabySpeed;
    public float SearchRange;
    public float SearchAngle;
    public GameObject HandStamp;
    public Transform[] HandPoints;
    private int handCount;
    private bool searchedWall;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeHandStamps());
        searchedWall = false;
    }
    private IEnumerator makeHandStamps()
    {
        while (true)
        {
            Transform tra = HandPoints[handCount = (handCount + 1) % HandPoints.Length];
            GameObject d = Instantiate(HandStamp) as GameObject;
            d.transform.position = tra.position;
            d.transform.rotation = Quaternion.LookRotation(Vector3.up, getForward());
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
        if (searchWall() )
        {
            if (!searchedWall)
            {
                searchedWall = true;
                print("クララが立った");
            }
        }
        else
        {
            searchedWall = false;
        }
    }
    private bool searchWall()
    {
        Vector3 fo = getForward();
        Ray ray = new Ray(this.transform.position, fo);
        RaycastHit hit = new RaycastHit();
        float searchRange = SearchRange + getRadiusZ();
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            return Vector3.Dot(-hit.normal, fo) > Mathf.Cos(SearchAngle * Mathf.Rad2Deg);
        }
        return false;
    }

    private float getRadiusZ()
    {
        BoxCollider boxCollider = this.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            return boxCollider.size.z / 2f;
        }
        return 0;
    }
    private void babymove()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float rv = Input.GetAxis("R_Stick_V");
        float rh = Input.GetAxis("R_Stick_H");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        this.transform.Rotate(0f, hor + rh + mouseX, 0f, Space.World);
        Camera.main.transform.Rotate(rv + mouseY, 0f, 0f, Space.Self);
        Vector3 fo = getForward();
        this.transform.position += fo * BabySpeed * Time.deltaTime * ver;
    }
}
