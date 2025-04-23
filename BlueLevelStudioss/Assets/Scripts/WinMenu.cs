using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 300, 200, 50), "VOLVER A JUGAR"))
        {
            Debug.Log("Cargando GameScene...");
            SceneManager.LoadScene("GameScene");
        }
        if (GUI.Button(new Rect(100, 100, 200, 50), "MENU"))
        {
            Debug.Log("Cargando MainMenu PAPU...");
            SceneManager.LoadScene("MainMenu");
        }
        if (GUI.Button(new Rect(100, 200, 200, 50), "SALIR"))
        {
            Debug.Log("Saliendo...");
            Application.Quit();
        }
    }
}
