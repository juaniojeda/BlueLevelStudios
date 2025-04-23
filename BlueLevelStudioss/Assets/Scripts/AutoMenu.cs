using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoMenu : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 200, 50), "JUGAR"))
        {
            Debug.Log("Cargando GameScene...");
            SceneManager.LoadScene("GameScene");
        }

        if (GUI.Button(new Rect(100, 200, 200, 50), "SALIR"))
        {
            Debug.Log("Saliendo...");
            Application.Quit();
        }
    }
}

