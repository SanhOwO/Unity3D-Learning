using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //重启游戏

public class Dialog : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            NetworkLauncher.PlayButton();
        }
    }

   
}
