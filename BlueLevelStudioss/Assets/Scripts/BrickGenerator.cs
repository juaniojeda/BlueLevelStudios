using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGenerator : MonoBehaviour
{
    public BrickPool pool;
    public int rows = 3;
    public int columns = 5;
    public float spacing = 0.1f;

    public void GenerateBricks()
    {
        Vector3 startPos = transform.position;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 pos = startPos + new Vector3(x * (1 + spacing), y * (0.5f + spacing), 0f);
                pool.GetBrick(pos);
            }
        }
    }
}
