using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiate : MonoBehaviour
{
    // 弾のPrefabを指定
    public GameObject CubePrefab;
    cameraMove cM;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                for (int k = -2; k <= 2; k++)
                {
                    Instantiate(CubePrefab, new Vector3(i * 10.0f, k * 10.0f + cam.transform.position.y, j * 10.0f + 50.0f), transform.rotation);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
