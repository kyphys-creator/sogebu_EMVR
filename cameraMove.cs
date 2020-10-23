using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    private float unitAccel = 0.1f;

    Vector3 ACCEL3;
    Vector4 ACCEL4;
    Vector3 accela3;
    Vector4 accela4;
    public Vector3 u3;
    public Vector4 u4;
    Vector3 u3hat;
    public Vector4 xx4;
    public Vector3 xx3;
    public Vector3 X3;
    public Vector4 X4;
    public Matrix4x4 Lplayer;
    public Matrix4x4 Linv;
    private float qom;
    public GameObject targetObject;
    public Matrix4x4 met;
    private Matrix4x4 aq;
    ArrowDirection ad;
    public Matrix4x4 R;
    private Matrix4x4 r;
    public Vector4 LForce;
    public List<Vector4> ppoL = new List<Vector4>();
    private Vector4 dx4;
    public Vector4 upt;
    private float a;
    private float b;
    private float c;
    private float sgm;
    public Vector4 xint;//intersection of worldline with PLC

    // Use this for initialization
    void Start()
    {
        ad = targetObject.GetComponent<ArrowDirection>();
        R = Matrix4x4.identity;
        xx4 = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0f);
        u3 = new Vector3(0.0f, 0.0f, 0.0f);
        u4 = new Vector4(u3.x, u3.y, u3.z, 1f);
        u3hat = u3.normalized;
        Lplayer = Matrix4x4.identity;
        met = Matrix4x4.identity;
        met.m33 = -1;
        qom = 0.01f;
        aq = ad.q;
        Debug.Log($"Q0={ad.q}");
        ppoL.Add(xx4);
        r = Matrix4x4.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aq = ad.q;
        Quaternion q = transform.rotation.normalized;
        //Defining Rotation Matrix by using Quartanion
        R.m00 = q.x * q.x - q.y * q.y - q.z * q.z + q.w * q.w;
        R.m01 = 2 * (q.x * q.y - q.z * q.w);
        R.m02 = 2 * (q.x * q.z + q.y * q.w);
        R.m10 = 2 * (q.x * q.z + q.y * q.w);
        R.m11 = -q.x * q.x + q.y * q.y - q.z * q.z + q.w * q.w;
        R.m12 = 2 * (q.y * q.z - q.x * q.w);
        R.m20 = 2 * (q.x * q.z - q.y * q.w);
        R.m21 = 2 * (q.y * q.z + q.x * q.w);
        R.m22 = -q.x * q.x - q.y * q.y + q.z * q.z + q.w * q.w;

        //Player's Input
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ACCEL3 = R * Vector3.up * unitAccel;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            ACCEL3 = R * Vector3.down * unitAccel;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            ACCEL3 = R * Vector3.forward * unitAccel;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ACCEL3 = R * Vector3.back * unitAccel;
        }
        else
        {
            ACCEL3 = Vector3.zero;
        }

        //L has upper and lower indices: (0,1,2,3) is (x,y,z,w), where w is t.
        Lplayer = LTrans(u3);

        Linv = Lplayer.inverse;

        //LForce is a vector in world coordinate space
        LForce = qom * (met * ad.f * u4);

        ACCEL4 = new Vector4(ACCEL3.x, ACCEL3.y, ACCEL3.z, 0);
        accela4 = Linv * ACCEL4 + LForce - u4.normalized * 0.01f;// 0.15f
        accela3 = new Vector3(accela4.x, accela4.y, accela4.z);

        u3 += accela3 * Time.deltaTime;
        //u3 *= 0.98f;
        u4 = new Vector4(u3.x, u3.y, u3.z, Mathf.Sqrt(1f + u3.sqrMagnitude));
        u3hat = u3.normalized;

        r.m00 = Mathf.Sqrt(1f - u3.sqrMagnitude);
        r.m11 = Mathf.Sqrt(1f - u3.sqrMagnitude);
        r.m22 = Mathf.Sqrt(1f - u3.sqrMagnitude);
        Shader.SetGlobalMatrix("R", r);

        xx4 += u4 * Time.deltaTime;
        xx3 = xx4;
        X4 = Lplayer * xx4;
        X3 = new Vector3(X4.x, X4.y, X4.z);
        transform.position = X3;

        //add a latest position to position list
        ppoL.Add(xx4);

        //debugging functions
        Debug.Log($"L={Lplayer}");
        Debug.Log($"AD.Q={aq}");
        Debug.Log($"addf={ad.f}");
        Debug.Log($"addq={ad.q}");
        Debug.Log($"LForce={LForce}");
        Debug.Log($"Linv={Linv}");
        Debug.Log($"ACCEL4={ACCEL4}");
        Debug.Log($"accel4={accela4}");
        Debug.Log($"u4={u4}");
        Debug.Log($"x4 ={xx4}");
        Debug.Log($"u4={u4}");
    }
    public Vector3 vp(Vector3 v1, Vector3 v2) //calculate vectorproduct
    {
        //private Vector3 v3;
        return new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }

    public Vector4 cont(Matrix4x4 m1, Vector4 v1)　//contraction between rank2 tensor and vector
    {
        return new Vector4(m1.m00 * v1.x + m1.m01 * v1.y + m1.m02 * v1.z + m1.m03 * v1.w, m1.m10 * v1.x + m1.m11 * v1.y + m1.m12 * v1.z + m1.m13 * v1.w, m1.m20 * v1.x + m1.m21 * v1.y + m1.m22 * v1.z + m1.m23 * v1.w, 0);
    }
    public Vector4 cor(Matrix4x4 m1, Vector4 v1)
    {
        return new Vector4(m1.m03 * v1.w, m1.m13 * v1.w, m1.m23 * v1.w, 0);
    }
    public float lip(Vector4 v1, Vector4 v2) //Lorentzian inner product
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z - v1.w * v2.w;
    }

    public float lSqN(Vector4 v) //Lorentzian squared norm
    {
        return v.x * v.x + v.y * v.y + v.z * v.z - v.w * v.w;
    }

    public Matrix4x4 LTrans(Vector3 u3)
    {
        Vector3 u3hat = u3.normalized;
        Vector4 u4 = new Vector4(u3.x, u3.y, u3.z, Mathf.Sqrt(1f + u3.sqrMagnitude));
        Matrix4x4 L = Matrix4x4.identity;
        //Lply has upper and lower indices: (0,1,2,3) is (x,y,z,w), where w is t.
        L.m00 = 1f + (u4.w - 1f) * u3hat.x * u3hat.x;
        L.m11 = 1f + (u4.w - 1f) * u3hat.y * u3hat.y;
        L.m22 = 1f + (u4.w - 1f) * u3hat.z * u3hat.z;

        L.m01 = (u4.w - 1f) * u3hat.x * u3hat.y;
        L.m02 = (u4.w - 1f) * u3hat.x * u3hat.z;
        L.m10 = (u4.w - 1f) * u3hat.y * u3hat.x;
        L.m12 = (u4.w - 1f) * u3hat.y * u3hat.z;
        L.m20 = (u4.w - 1f) * u3hat.z * u3hat.x;
        L.m21 = (u4.w - 1f) * u3hat.z * u3hat.y;
        L.m03 = (-1) * u4.x;
        L.m13 = (-1) * u4.y;
        L.m23 = (-1) * u4.z;
        L.m30 = (-1) * u4.x;
        L.m31 = (-1) * u4.y;
        L.m32 = (-1) * u4.z;

        L.m33 = u4.w;
        return L;
    }

}