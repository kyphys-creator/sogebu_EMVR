using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection2 : MonoBehaviour
{
    private Vector3 pointchargeposworldframe3;

    private Vector3 pointcharge2posworldframe3;

    private Vector3 efield;

    private float ep = 0.000038f;
    private float q1;
    private float q2;
    private float m1;
    private float m2;

    public Matrix4x4 F;
    public Matrix4x4 G;


    Camera cam;
    cameraMove2 cameraMove2;
    cubeMove2 cubeMove2;
    cubeMove2 cubeMove2pointcharge1;
    cubeMove2 cubeMove2pointcharge2;
    // Start is called before the first frame update
    void Start()
    {
        //importing section for Electromagnetic Effects
        cam = Camera.main;
        cameraMove2 = cam.GetComponent<cameraMove2>();
        cubeMove2 = this.GetComponent<cubeMove2>();
        cubeMove2pointcharge1 = GameObject.Find("Point_charge").GetComponent<cubeMove2>();
        cubeMove2pointcharge2 = GameObject.Find("Point_charge2").GetComponent<cubeMove2>();

        //Update the latest positions of Each point source
        pointchargeposworldframe3 = cubeMove2pointcharge1.objposworldframe3;
        pointcharge2posworldframe3 = cubeMove2pointcharge2.objposworldframe3;

        //Electric charge of the point source.
        q1 = 0.0f;
        q2 = 0.0f;
        m1 = 1.0f;
        m2 = 0.0f;
        //an Arrow's Position Vector mesured from Each point source in World frame
        Vector3 r = cubeMove2pointcharge1.Lobject * rR(cubeMove2.objposworldframe3, pointchargeposworldframe3);
        Vector3 R = cubeMove2pointcharge2.Lobject * rR(cubeMove2.objposworldframe3, pointcharge2posworldframe3);

        //Player's Position Vector mesured from Each point source in World frame
        Vector3 rp = cubeMove2pointcharge1.Lobject * rR(cameraMove2.playrposworldframe3, pointchargeposworldframe3);
        Vector3 Rp = cubeMove2pointcharge2.Lobject * rR(cameraMove2.playrposworldframe3, pointcharge2posworldframe3);

        //Creating Electromagnetic Tensor for an Arrow's position in world frame
        Matrix4x4 F1 = cubeMove2pointcharge1.Lobject * K(field(cubeMove2pointcharge1.Lobject * r, q1), field(cubeMove2pointcharge1.Lobject * r, m1)) * cubeMove2pointcharge1.Lobject.transpose;
        Matrix4x4 F2 = cubeMove2pointcharge2.Lobject * K(field(cubeMove2pointcharge1.Lobject * R, q2), field(cubeMove2pointcharge1.Lobject * R, m2)) * cubeMove2pointcharge2.Lobject.transpose;
        F = Sum(F1, F2);
        //Creating Electromagnetic Tensor for an Arrow's position in Player's rest frame
        Matrix4x4 f = cameraMove2.Lplayer.transpose * F * cameraMove2.Lplayer;

        efield = new Vector3(f.m03, f.m13, f.m23);
        this.transform.localScale = new Vector3(1, efield.magnitude / 20, 1);

        if (vp(efield, new Vector3(0, 0, 1)) == new Vector3(0, 0, 0))
        {
            Vector3 up1 = vp(efield, new Vector3(0, 1, 0));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, efield);
            transform.rotation = erotation;
        }
        else
        {
            Vector3 up1 = vp(efield, new Vector3(0, 0, 1));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, efield);
            transform.rotation = erotation;
        }

        //Creating Electromagnetic Tensor for Player's position in world frame
        Matrix4x4 G1 = cubeMove2pointcharge1.Lobject * K(field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * rp, q1), field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * rp, m1)) * cubeMove2pointcharge1.Lobject.transpose;
        Matrix4x4 G2 = cubeMove2pointcharge2.Lobject * K(field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * Rp, q2), field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * Rp, m2)) * cubeMove2pointcharge2.Lobject.transpose;
        G = Sum(G1, G2);
    }

    // Update is called once per frame
    void Update()
    {
        //Update the latest positions of Each point source
        pointchargeposworldframe3 = cubeMove2pointcharge1.objposworldframe3;
        pointcharge2posworldframe3 = cubeMove2pointcharge2.objposworldframe3;

        //an Arrow's Position Vector mesured from Each point source in World frame
        Vector3 r = cubeMove2pointcharge1.Lobject * rR(cubeMove2.objposworldframe3, pointchargeposworldframe3);
        Vector3 R = cubeMove2pointcharge2.Lobject * rR(cubeMove2.objposworldframe3, pointcharge2posworldframe3);

        //Player's Position Vector mesured from Each point source in World frame
        Vector3 rp = cubeMove2pointcharge1.Lobject * rR(cameraMove2.playrposworldframe3, pointchargeposworldframe3);
        Vector3 Rp = cubeMove2pointcharge2.Lobject * rR(cameraMove2.playrposworldframe3, pointcharge2posworldframe3);

        //Creating Electromagnetic Tensor for an Arrow's position in world frame
        Matrix4x4 F1 = cubeMove2pointcharge1.Lobject * K(field(cubeMove2pointcharge1.Lobject * r, q1), field(cubeMove2pointcharge1.Lobject * r, m1)) * cubeMove2pointcharge1.Lobject.transpose;
        Matrix4x4 F2 = cubeMove2pointcharge2.Lobject * K(field(cubeMove2pointcharge1.Lobject * R, q2), field(cubeMove2pointcharge1.Lobject * R, m2)) * cubeMove2pointcharge2.Lobject.transpose;
        F = Sum(F1, F2);
        //Creating Electromagnetic Tensor for an Arrow's position in Player's rest frame
        Matrix4x4 f = cameraMove2.Lplayer.transpose * F * cameraMove2.Lplayer;

        efield = new Vector3(f.m03, f.m13, f.m23);
        this.transform.localScale = new Vector3(1, efield.magnitude / 10, 1);

        if (vp(efield, new Vector3(0, 0, 1)) == new Vector3(0, 0, 0))
        {
            Vector3 up1 = vp(efield, new Vector3(0, 1, 0));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, efield);
            transform.rotation = erotation;
        }
        else
        {
            Vector3 up1 = vp(efield, new Vector3(0, 0, 1));
            // 方向を、回転情報に変換
            Quaternion erotation = Quaternion.LookRotation(up1, efield);
            transform.rotation = erotation;
        }

        //Creating Electromagnetic Tensor for Player's position in world frame
        Matrix4x4 G1 = cubeMove2pointcharge1.Lobject * K(field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * rp, q1), field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * rp, m1)) * cubeMove2pointcharge1.Lobject.transpose;
        Matrix4x4 G2 = cubeMove2pointcharge2.Lobject * K(field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * Rp, q2), field(cameraMove2.LTrans(cubeMove2pointcharge1.objvelworldframe3) * Rp, m2)) * cubeMove2pointcharge2.Lobject.transpose;
        G = Sum(G1, G2);
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
    public Matrix4x4 Sum(Matrix4x4 M1, Matrix4x4 M2)
    {
        Matrix4x4 Sum = Matrix4x4.identity;
        Sum.m00 = M1.m00 + M2.m00;
        Sum.m01 = M1.m01 + M2.m01;
        Sum.m02 = M1.m02 + M2.m02;
        Sum.m03 = M1.m03 + M2.m03;
        Sum.m10 = M1.m10 + M2.m10;
        Sum.m11 = M1.m11 + M2.m11;
        Sum.m12 = M1.m12 + M2.m12;
        Sum.m13 = M1.m13 + M2.m13;
        Sum.m20 = M1.m20 + M2.m20;
        Sum.m21 = M1.m21 + M2.m21;
        Sum.m22 = M1.m22 + M2.m22;
        Sum.m23 = M1.m23 + M2.m23;
        Sum.m30 = M1.m30 + M2.m30;
        Sum.m31 = M1.m31 + M2.m31;
        Sum.m32 = M1.m32 + M2.m32;
        Sum.m33 = M1.m33 + M2.m33;
        return Sum;
    }
}
