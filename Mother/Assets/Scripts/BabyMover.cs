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
    private Animator CameraAC;
    public AnimationClip StandUpClip;
    private int handCount;
    private bool searchedWall;
    public bool canOperate;
    public LayerMask Mask;
    public float TurnBackTime;
    private bool isTurning;
    public GameObject OtherCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeHandStamps());
        searchedWall = false;
        canOperate = true;
        CameraAC = this.GetComponent<Animator>();
        isTurning = false;
    }
    private IEnumerator makeHandStamps()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Transform tra = HandPoints[handCount = (handCount + 1) % HandPoints.Length];
            RaycastHit hit = new RaycastHit();
            if (!Physics.Raycast(tra.position, -tra.up, out hit, SearchRange * 3f, Mask)) continue;
            GameObject d = Instantiate(HandStamp) as GameObject;
            d.GetComponentInChildren<Animator>().SetBool("Scratching", CameraAC.GetBool("Standing"));
            d.transform.position = hit.point + hit.normal * 0.01f;
            d.transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.Cross(this.transform.right, hit.normal));
            d.transform.localScale = tra.localScale * d.transform.localScale.x;
            Destroy(d, 10f);
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
        Vector3 fo = getForward();
        Ray ray = new Ray(this.transform.position, fo);
        RaycastHit hit = new RaycastHit();
        float searchRange = SearchRange + getRadiusZ();
        if (Physics.Raycast(ray, out hit, searchRange, Mask))
        {
            if (hit.collider.tag == "CanClimb" && CameraAC.GetBool("Standing"))
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    canOperate = false;
                    StartCoroutine(Climbing(this.transform.forward * 0.4f + Vector3.up * hit.collider.GetComponent<BoxCollider>().size.y));
                }
            }
        }
        babymove();
        if (!canOperate) return;
        if (searchWall())
        {
            if (!searchedWall)
            {
                searchedWall = true;
                canOperate = false;
                StartCoroutine(lookWall());
            }
        }
        else
        {
            if (CameraAC.GetBool("Standing"))
            {
                CameraAC.SetBool("Standing", false);
            }
            searchedWall = false;
            GameManager.gameManager.SetClimb(false);
        }

    }

    private IEnumerator Climbing(Vector3 relative)
    {
        isTurning = true;
        CameraAC.SetBool("Standing", false);
        CameraAC.SetTrigger("GoIdle");
        OtherCamera.GetComponent<Camera>().depth = 1;
        OtherCamera.transform.position = Camera.main.transform.position;
        OtherCamera.transform.rotation = Camera.main.transform.rotation;
        float starttime = Time.time;
        Vector3 startpoint = Camera.main.transform.position;
        Vector3 goalpoint = this.transform.position + relative;
        float usetime = (goalpoint - startpoint).magnitude / BabySpeed;
        this.transform.position += relative;
        while (true)
        {
            float nowtime = Time.time - starttime;
            if (nowtime > usetime) break;
            OtherCamera.transform.position = Vector3.Lerp(startpoint, goalpoint, nowtime / usetime);
            yield return null;
        }
        OtherCamera.GetComponent<Camera>().depth = -2;
        isTurning = false;
        this.GetComponent<CapsuleCollider>().enabled = true;
        canOperate = true;
    }
    private bool searchWall()
    {
        Vector3 fo = getForward();
        Ray ray = new Ray(this.transform.position, fo);
        RaycastHit hit = new RaycastHit();
        float searchRange = SearchRange + getRadiusZ();
        if (Physics.Raycast(ray, out hit, searchRange, Mask))
        {
            if (hit.collider.tag == "CanClimb")
            {
                GameManager.gameManager.SetClimb(true);
            }
            if (hit.collider.tag == "CanMove") return false;
            return Vector3.Dot(-hit.normal, fo) > Mathf.Cos(SearchAngle * Mathf.Deg2Rad);
        }
        return false;
    }
    public Vector3 getWallForward()
    {
        Ray ray = new Ray(this.transform.position, getForward());
        RaycastHit hit = new RaycastHit();
        float searchRange = SearchRange + getRadiusZ();
        Physics.Raycast(ray, out hit, searchRange, Mask);
        return -hit.normal;
    }
    private IEnumerator lookWall()
    {
        Vector3 sv = getForward();
        Vector3 tv = getWallForward();
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
        yield return new WaitForSeconds(StandUpClip.length);
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
        SphereCollider sphereCollider = this.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            return sphereCollider.radius;
        }
        CapsuleCollider capsuleCollider = this.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            return capsuleCollider.radius;
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

    public void TurnBack()
    {
        if (isTurning) return;
        isTurning = true;
        canOperate = false;
        StartCoroutine(turnBack());
    }

    private IEnumerator turnBack()
    {
        float startTime = Time.time;
        Vector3 target = -this.transform.forward;
        while (true)
        {
            if (Vector3.Dot(this.transform.forward, target) > 0.99f) break;
            this.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(this.transform.forward, target, Time.deltaTime / TurnBackTime, 0f));
            yield return null;
        }
        this.transform.rotation = Quaternion.LookRotation(target);
        canOperate = true;
        isTurning = false;
    }
}
