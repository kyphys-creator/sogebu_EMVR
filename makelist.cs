using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makelist : MonoBehaviour
{
    public GameObject targetObject;
    public List<Vector4> cubelis = new List<Vector4>();
    cubeMove bM;
    public Vector4 xx4;
    public Vector4 dx4;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        bM = targetObject.GetComponent<cubeMove>();
        xx4 = bM.x4;
        //make a list of position vector
        cubelis.Add(xx4);
        Debug.Log($"List={cubelis.Count}");
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cubelis.Add(bM.x4);
        Debug.Log($"List={cubelis.Count}");
    }
}
