using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeMove : MonoBehaviour
{
    public Vector4 x4;
    public Vector4 xCam4;
    private Vector4 n4;
    public Vector4 xnorm;
    float t;
    public Vector4 xx4;
    public List<Vector4> cubelis = new List<Vector4>();
    makelist mk;
    private Vector4 dx4;
    public Vector4 ut;
    private float a;
    private float b;
    private float c;
    private float sgm;
    private int j;
    public Vector4 xint;//intersection of worldline with PLC
    Camera cam;
    cameraMove cM;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        xCam4 = cM.playrposworldframe4;

        x4 = cM.Lplayerinverse * new Vector4(transform.position.x, transform.position.y, transform.position.z, -(cam.transform.position - transform.position).magnitude);
        xx4 = x4;
        //make a list of position vector
        j = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*int i = j;

        while (true)
        {
            if (lSqN(cM.xx4 - mk.cubelis[i]) >= 0)
            {
                break;
            }
            i++;
        }
        //evaluate displacement vector
        dx4 = mk.cubelis[i + 1] - mk.cubelis[i];
        //calculate velocity according to the displacement vector
        ut = dx4 / Mathf.Sqrt(-lSqN(dx4));
        a = lSqN(cM.xx4 - cubelis[i]);
        b = lip(cM.xx4 - mk.cubelis[i], mk.cubelis[i + 1] - mk.cubelis[i]);
        c = lSqN(mk.cubelis[i + 1] - mk.cubelis[i]);
        sgm = (b - Mathf.Sqrt(Mathf.Pow(b, 2) - a * c)) / a;
        //intersecting point with player's PLC by linear interpolation
        xint = (1.0f - sgm) * mk.cubelis[i] + sgm * mk.cubelis[i + 1];
        Debug.Log($"xint={xint}");
        x4 = xint;
        j = i + 1;*/
        x4 = cM.Lplayerinverse * new Vector4(transform.position.x, transform.position.y, transform.position.z, -(cam.transform.position - transform.position).magnitude);
        xCam4 = cM.playrposworldframe4;
        t = lip(n4, x4 - xCam4) - Mathf.Sqrt(Mathf.Pow(lip(n4, x4 - xCam4), 2) + lSqN(x4 - xCam4));
        x4.w += cM.playrvelworldframe4.w * Time.deltaTime;
        transform.position = cM.Lplayer * x4;
        xnorm = xCam4.normalized;
    }

    public float lip(Vector4 v1, Vector4 v2) //Lorentzian inner product
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z - v1.w * v2.w;
    }

    public float lSqN(Vector4 v) //Lorentzian squared norm
    {
        return v.x * v.x + v.y * v.y + v.z * v.z - v.w * v.w;
    }
}