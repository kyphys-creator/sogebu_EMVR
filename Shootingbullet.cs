using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootingbullet : MonoBehaviour
{
    public GameObject bullet;//bullet prefab
    public Vector4 muzzle;//position of player's muzzle
    public float bmag;//velocitymagnitude fo the bullet
    public Vector4 vel;
    public Vector4 bullet4;
    private Vector3 U3hat;
    private Vector4 U4;
    public Vector3 v;
    private Matrix4x4 R;//rotation matix
    private Matrix4x4 Ll;
    private Matrix4x4 l;
    private float timeElapsed;
    private float timeOut;
    Camera cam;
    cameraMove cM;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        muzzle = cam.transform.position;
        bullet4 = new Vector4(muzzle.x, muzzle.y, muzzle.z, 0.0f);
        v = new Vector3(0.0f, 0.0f, 1.0f);
        bmag = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = cam.transform.rotation.normalized;
        //Defining Rotation matrix
        R.m00 = q.x * q.x - q.y * q.y - q.z * q.z + q.w * q.w;
        R.m01 = 2 * (q.x * q.y - q.z * q.w);
        R.m02 = 2 * (q.x * q.z + q.y * q.w);
        R.m10 = 2 * (q.x * q.z + q.y * q.w);
        R.m11 = -q.x * q.x + q.y * q.y - q.z * q.z + q.w * q.w;
        R.m12 = 2 * (q.y * q.z - q.x * q.w);
        R.m20 = 2 * (q.x * q.z - q.y * q.w);
        R.m21 = 2 * (q.y * q.z + q.x * q.w);
        R.m22 = -q.x * q.x - q.y * q.y + q.z * q.z + q.w * q.w;

        Ll = cM.Lplayerinverse;
        U3hat = v.normalized;
        U4 = new Vector4(U3hat.x, U3hat.y, U3hat.z, Mathf.Sqrt(1f + U3hat.sqrMagnitude));
        //duplication of bullets
        vel = bmag * v;
        vel = Ll * vel;

        l.m00 = 1f + (U4.w - 1f) * U3hat.x * U3hat.x;
        l.m11 = 1f + (U4.w - 1f) * U3hat.y * U3hat.y;
        l.m22 = 1f + (U4.w - 1f) * U3hat.z * U3hat.z;

        l.m01 = (U4.w - 1f) * U3hat.x * U3hat.y;
        l.m02 = (U4.w - 1f) * U3hat.x * U3hat.z;
        l.m10 = (U4.w - 1f) * U3hat.y * U3hat.x;
        l.m12 = (U4.w - 1f) * U3hat.y * U3hat.z;
        l.m20 = (U4.w - 1f) * U3hat.z * U3hat.x;
        l.m21 = (U4.w - 1f) * U3hat.z * U3hat.y;

        l.m03 = (-1) * U4.x;
        l.m13 = (-1) * U4.y;
        l.m23 = (-1) * U4.z;
        l.m30 = (-1) * U4.x;
        l.m31 = (-1) * U4.y;
        l.m32 = (-1) * U4.z;

        l.m33 = U4.w;

        vel = R * l * vel;

        v = bmag * new Vector3(vel.x, vel.y, vel.z);
        Vector4 vv = R * cam.transform.forward;
        muzzle = cam.transform.position + bmag * new Vector3(vv.x, vv.y, vv.z);


        //if z key is pusheds
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullets = Instantiate(bullet, muzzle, cam.transform.rotation) as GameObject;
            //発射ベクトル
            Vector3 force;
            //発射の向きと速度を決定
            force = bullets.transform.forward * 1.0f;
            // Rigidbodyに力を加えて発射
            bullets.GetComponent<Rigidbody>().AddForce(force);

            Destroy(bullets, 0.0f);
        }
    }
}