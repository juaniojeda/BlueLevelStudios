using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backtomenu : MonoBehaviour
{

  void OnGUI()
    {

        if (GUI.Button(new Rect(100, 100, 200, 50), "VOLVER AL MENU"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


