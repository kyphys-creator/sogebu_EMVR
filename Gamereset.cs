using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
 
 public class Gamereset : MonoBehaviour
{
    // Update is called once per frame
    //　ゲーム終了ボタンを押したら実行する
    public void EndGame()
    {
		Application.Quit();
    }
}