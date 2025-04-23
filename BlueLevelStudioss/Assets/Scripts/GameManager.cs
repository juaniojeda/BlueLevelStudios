using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BrickGenerator brickGenerator;

    private void Awake()
    {
        brickGenerator.GenerateBricks();
    }
}
