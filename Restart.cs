using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Restart : MonoBehaviour
{
    // Update is called once per frame
    public void RestartGame()
    {
        SceneManager.LoadScene("20200626");
    }
}
