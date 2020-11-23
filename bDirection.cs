using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bDirection : MonoBehaviour
{
    public Vector3 ef;
    public Vector3 efp;
    public Vector3 bf;
    public Vector3 bfp;
    public Vector3 Rzero;
    public Vector3 rzero;
    private Vector3 R;
    private Vector3 r;
    private float Rmag;
    private float rmag;
    private Matrix4x4 F;
    private Matrix4x4 f;
    public Matrix4x4 Q;
    public Matrix4x4 q;
    private Matrix4x4 l;
    private Matrix4x4 lt;
    private float emag1;
    private float bmag1;
    private Vector3 up1;
    private float q1;
    private float q2;
    private float ep;
    private Vector3 rp;
    private Vector3 Rp;
    private float t;
    Camera cam;
    cameraMove cM;
    public GameObject tgObject;
    private Matrix4x4 aq;
    ArrowDirection ad;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 tmp = GameObject.Find("Point_charge").transform.position;
        Vector3 tmp2 = GameObject.Find("Point_charge2").transform.position;
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        //Defining a point charge at Rzero.
        Rzero = tmp;
        rzero = tmp2;
        //Electric charge of the point source.
        q1 = 1.0f;
        q2 = 0.0f;

        R = rR(transform.position, Rzero);
        r = rR(transform.position, rzero);

        Rmag = R.magnitude;
        rmag = r.magnitude;

        ep = 0.000038f;

        //Defining electric field so that arrow can be drawn.
        ef = field(r, q1) + field(R, q2);
        //efield = new Vector3(0.0f, 0.0f, 23.0f);
        bf = field(r, 0) + field(R, 0);


        Debug.Log($"efield02={ef}");
        Debug.Log($"bfield02={bf}");

        //defining elecromagnetic vector
        //creating electromagnetic tensor
        //F has both lower indices: (0,1,2,3) is (x,y,z,t), where t=w.
        F = K(ef, bf);

        Debug.Log($"Fad2={F}");
        t = 0;
        Q = F;
        this.transform.localScale = new Vector3(1, bf.magnitude / 10, 1);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*t += Time.deltaTime;
        q1 = Mathf.Cos(cM.u4.w * t);
        q2 = -Mathf.Cos(cM.u4.w * t);
        //Defining electric field so that arrow can be drawn.
        ef = field(r, R, q1, q2);
        efield = new Vector3(0.0f, 0.0f, 23.0f);
        bf = field(r, R, 0, 0);

        //defining elecromagnetic vector
        //creating electromagnetic tensor
        //F has both lower indices: (0,1,2,3) is (x,y,z,t), where t=w.
        F = K(ef, bf);*/

        //referencing lorentzian matrix
        l = cM.Lplayerinverse;

        //Q = F;

        lt = l.transpose;
        Q = lt * F * l;
        //alternating direction of arrow
        ef = new Vector3(Q.m03, Q.m13, Q.m23);
        bf = new Vector3(Q.m12, Q.m20, Q.m01);

        emag1 = ef.magnitude;
        bmag1 = bf.magnitude;

        if (vp(bf, new Vector3(0, 0, 1)) == new Vector3(0, 0, 0))
        {
            up1 = vp(bf, new Vector3(0, 1, 0));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, bf);
            transform.rotation = erotation;
        }
        else
        {
            up1 = vp(bf, new Vector3(0, 0, 1));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, bf);
            transform.rotation = erotation;
        }


        //float speed = 0.1f;
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        //this.transform.forward = (unit vector along z axis)
        //Quaternion.Slerp(this.transform.rotation, erotation, speed);

        //pos.x = l.m00 * pos.x + l.m01 * pos.y + l.m02 * pos.z;
        //pos.y = l.m10 * pos.x + l.m11 * pos.y + l.m12 * pos.z;
        //pos.z = l.m20 * pos.x + l.m21 * pos.y + l.m22 * pos.z;
        //transform.position = pos;
        this.transform.localScale = new Vector3(1, bmag1 / 10, 1);

        Debug.Log($"E={ef}");
        Debug.Log($"B={bf}");
        Debug.Log($"Qad={Q}");
    }
    public Vector3 vp(Vector3 v1, Vector3 v2) //calculate vectorproduct
    {
        //private Vector3 v3;
        return new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }

    public Vector3 rR(Vector3 v1, Vector3 v2)
    {
        return v1 - v2;
    }

    public Vector3 field(Vector3 r, float q1)
    {
        return (q1 / (ep * Mathf.Pow(r.magnitude, 3.0f))) * r;
    }

    public Matrix4x4 K(Vector3 v1, Vector3 v2)
    {
        Matrix4x4 K = Matrix4x4.identity;

        K.m00 = 0;
        K.m03 = v1.x;
        K.m13 = v1.y;
        K.m23 = v1.z;

        K.m10 = -v2.z;
        K.m11 = 0;
        K.m01 = v2.z;
        K.m02 = -v2.y;
        K.m20 = v2.y;
        K.m12 = v2.x;
        K.m22 = 0;
        K.m21 = -v2.x;

        K.m30 = -v1.x;
        K.m31 = -v1.y;
        K.m32 = -v1.z;
        K.m33 = 0;

        return K;
    }
}