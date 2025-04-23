using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Vector3 Position => transform.position;
    public Vector3 Size => transform.localScale;

    public void DestroyBrick()
    {
        Destroy(gameObject);
    }
}
