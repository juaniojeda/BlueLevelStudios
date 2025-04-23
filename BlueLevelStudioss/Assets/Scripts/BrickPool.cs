using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPool : MonoBehaviour
{
    public GameObject brickPrefab;
    public int poolSize = 30;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject brick = Instantiate(brickPrefab);
            brick.SetActive(false);
            pool.Add(brick);
        }
    }

    public GameObject GetBrick(Vector3 position)
    {
        foreach (var brick in pool)
        {
            if (!brick.activeInHierarchy)
            {
                brick.transform.position = position;
                brick.SetActive(true);
                return brick;
            }
        }

        // Si no hay ladrillos disponibles, opcionalmente expandís el pool:
        GameObject newBrick = Instantiate(brickPrefab, position, Quaternion.identity);
        pool.Add(newBrick);
        return newBrick;
    }
}
