using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class velocity : MonoBehaviour
{
    cameraMove cM;
    public Vector3 k3;
    Camera cam;

    //********** 開始 **********//
    public Text veltext; //Text用変数
    public double m3 = 0; //スコア計算用変数
    //********** 終了 **********//
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        veltext.text = "Velocity: 0% of the speed of light."; //初期スコアを代入して画面に表示
    }

    // Update is called once per frame
    void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        k3 = cM.u3;
        m3 = k3.magnitude;
        veltext.text = "Velocity: " + m3.ToString() + "% of the speed of light.";
    }
}
