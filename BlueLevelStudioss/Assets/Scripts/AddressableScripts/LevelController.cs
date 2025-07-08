using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private readonly Transform _nivelParent;

    public LevelController(Transform nivelContainer)
    {
        _nivelParent = nivelContainer;
    }

    public void LoadLevel(int levelNumber)
    {
        // 1) destruyo SOLO el prefab anterior
        foreach (Transform child in _nivelParent)
            GameObject.Destroy(child.gameObject);

        // 2) instancio el nuevo prefab de nivel
        var prefab = AssetsManager.Instance.GetInstance($"Nivel{levelNumber}");
        if (prefab == null) return;
        var lvlGO = GameObject.Instantiate(prefab, _nivelParent);
        lvlGO.name = $"Nivel{levelNumber}";
    }
}

//public class LevelController
//{
//    private readonly Transform _root;

//    public LevelController(Transform levelRoot)
//    {
//        _root = levelRoot;
//    }

//    public void LoadLevel(int levelNumber)
//    {
//        string key = $"Nivel{levelNumber}";
//        GameObject prefab = AssetsManager.Instance.GetInstance(key);
//        if (prefab == null)
//        {
//            Debug.LogError($"LevelController: no encontró '{key}'");
//            return;
//        }

//        // Limpia el nivel anterior
//        foreach (Transform child in _root)
//            GameObject.Destroy(child.gameObject);

//        // Instancia el nuevo nivel
//        var lvlGO = GameObject.Instantiate(prefab, _root);
//        lvlGO.name = key;
//    }
//}
//public class LevelController
//{
//    private readonly Transform _levelRoot;

//    public LevelController(Transform levelRoot)
//    {
//        _levelRoot = levelRoot;
//    }

//    /// <summary>
//    /// Instancia el prefab de nivel indicado por su número (1-based).
//    /// </summary>
//    public void LoadLevel(int levelNumber)
//    {
//        string key = $"Nivel{levelNumber}";
//        GameObject prefab = AssetsManager.Instance.GetInstance(key);
//        if (prefab != null)
//        {
//            // Opcional: destruye el nivel anterior
//            foreach (Transform child in _levelRoot) Destroy(child.gameObject);

//            // Instancia bajo un contenedor para mantener la escena limpia
//            var levelGO = GameObject.Instantiate(prefab, _levelRoot);
//            levelGO.name = key;
//        }
//        else
//        {
//            Debug.LogError($"LevelController: no existe el asset '{key}'");
//        }
//    }
//}

