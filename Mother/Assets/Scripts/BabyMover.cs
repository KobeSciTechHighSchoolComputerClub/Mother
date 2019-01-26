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
    public Animator CameraAC;
    private int handCount;
    private bool searchedWall;
    private bool canOperate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeHandStamps());
        searchedWall = false;
        canOperate = true;
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
        if (!canOperate)
        {
            return;
        }
        babymove();
        if (searchWall() )
        {
            if (!searchedWall)
            {
                searchedWall = true;
                StartCoroutine(lookWall());
            }
        }
        else
        {
            searchedWall = false;
            CameraAC.SetBool("Standing", false);
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
            return Vector3.Dot(-hit.normal, fo) > Mathf.Cos(SearchAngle * Mathf.Deg2Rad);
        }
        return false;
    }
    public Vector3 getWallForward()
    {
        Ray ray = new Ray(this.transform.position, getForward());
        RaycastHit hit = new RaycastHit();
        float searchRange = SearchRange + getRadiusZ();
        Physics.Raycast(ray, out hit, searchRange);
        return -hit.normal;
    }
    private IEnumerator lookWall()
    {
        Vector3 sv = getForward();
        Vector3 tv = getWallForward();
        tv.y = 0f;
        tv.Normalize();
        canOperate = false;
        float usetime = Mathf.Abs(Mathf.Acos(Vector3.Dot(tv, sv)) * Mathf.Rad2Deg / 50f);
        float startTime = Time.time;
        while (true)
        {
            if (Time.time - startTime > usetime) break;
            this.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(sv, tv, (Time.time - startTime) / usetime), Vector3.up);
            yield return null;
        }
        this.transform.rotation = Quaternion.LookRotation(tv, Vector3.up);
        CameraAC.SetBool("Standing", true);
        yield return null;
        StartCoroutine(WaitAnimationEnd("StandUp"));
    }
    private IEnumerator WaitAnimationEnd(string animatorName)
    {
        bool finish = false;
        while (!finish)
        {
            AnimatorStateInfo nowState = CameraAC.GetCurrentAnimatorStateInfo(0);
            if (nowState.IsName(animatorName))
            {
                finish = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        canOperate = true;
    }
    public void SetCanOperate(bool flag)
    {
        canOperate = flag;
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
