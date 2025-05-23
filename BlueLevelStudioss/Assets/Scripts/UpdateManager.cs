using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance { get; private set; }

    private List<ICustomUpdate> updatables = new List<ICustomUpdate>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(ICustomUpdate obj)
    {
        if (!updatables.Contains(obj))
            updatables.Add(obj);
    }

    public void Unregister(ICustomUpdate obj)
    {
        if (updatables.Contains(obj))
            updatables.Remove(obj);
    }

    void LateUpdate() // solo una vez por frame
    {
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < updatables.Count; i++)
        {
            // null-check por seguridad si fue desregistrado
            if (updatables[i] != null)
                updatables[i].CustomUpdate(deltaTime);
        }
    }
}
