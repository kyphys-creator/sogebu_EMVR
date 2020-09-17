using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellow_colour : MonoBehaviour
{
    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {
        //オブジェクトの色を赤に変更する
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
