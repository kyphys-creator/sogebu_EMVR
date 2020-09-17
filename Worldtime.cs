using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Worldtime : MonoBehaviour
{
    cameraMove cM;
    public Vector4 vw3;
    Camera cam;
    public float t;
    //********** 開始 **********//
    public Text worldtime; //Text用変数
    public double m3 = 0; //スコア計算用変数
                          //********** 終了 **********//

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        worldtime.text = "Worldtime:  0s"; //初期スコアを代入して画面に表示
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        vw3 = cM.u4;
        m3 = vw3.w * t;
        worldtime.text = "Worldtime:  " + m3.ToString() + "s";
    }
}
