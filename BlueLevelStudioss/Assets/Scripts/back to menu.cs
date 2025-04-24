using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backtomenu : MonoBehaviour
{

  void OnGUI()
    {

        if (GUI.Button(new Rect(100, 100, 400, 100), "VOLVER AL MENU"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


