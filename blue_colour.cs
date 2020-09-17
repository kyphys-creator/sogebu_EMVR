using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blue_colour : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //オブジェクトの色を赤に変更する
        GetComponent<Renderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
