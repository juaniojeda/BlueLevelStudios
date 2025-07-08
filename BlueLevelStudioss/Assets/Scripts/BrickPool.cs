using System.Collections.Generic;
using UnityEngine;

public class BrickPool
{
    private GameObject brickPrefab;
    private int poolSize;
    private List<GameObject> pool;

    public BrickPool(GameObject prefab, int initialSize)
    {
        brickPrefab = prefab;
        poolSize = initialSize;
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject brick = Object.Instantiate(brickPrefab);
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

        GameObject newBrick = Object.Instantiate(brickPrefab, position, Quaternion.identity);
        pool.Add(newBrick);
        return newBrick;
    }

    public void ReturnAllBricks()
    {
        foreach (var brick in pool)
            brick.SetActive(false);
    }
}
