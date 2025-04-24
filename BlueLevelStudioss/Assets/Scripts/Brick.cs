using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brick : MonoBehaviour
{
    public Vector3 Position => transform.position;
    public Vector3 Size => transform.localScale;

    public GameObject powerUpPrefab;
    public Transform paddle;
    public GameObject ballPrefab;
    public void DeactivateBrick()
    {
        gameObject.SetActive(false);

        if (Random.value < 0.05f)
        {
            GameObject powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            PowerUpMultiball pu = powerUp.GetComponent<PowerUpMultiball>();
            pu.paddle = paddle;
            pu.ballPrefab = ballPrefab;
        }
    }
}
