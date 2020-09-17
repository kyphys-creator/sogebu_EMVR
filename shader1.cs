using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shader1 : MonoBehaviour
{
    public Material[] sky;
    int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = sky[num];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
