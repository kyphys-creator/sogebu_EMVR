using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowcolour : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        //オブジェクトの色を赤に変更する
       GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
}