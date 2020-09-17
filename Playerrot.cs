using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerrot : MonoBehaviour
{
    public GameObject Player;
    [SerializeField]
    private float x_sensitivity = 3f;
    private float y_sensitivity = 3f;
    public Matrix4x4 R;

    // Start is called before the first frame update
    void Start()
    {
        R = Matrix4x4.identity;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = transform.rotation.normalized;
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

        Vector3 playerPos = Player.transform.position;
        float x_mouse = Input.GetAxis("Mouse X");
        //x_sensitivity = 3.0f;
        float angle_x = x_mouse * x_sensitivity;
        transform.RotateAround(playerPos, Vector3.up, angle_x);
        //float angle_x = x_sensitivity;

        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(playerPos, Vector3.up, angle_x);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(playerPos, Vector3.up, -angle_x);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(playerPos, Vector3.right, angle_x);
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(playerPos, Vector3.right, -angle_x);
        }else
        {
            transform.RotateAround(playerPos, Vector3.up, 0.0f);
        }*/

    }
}