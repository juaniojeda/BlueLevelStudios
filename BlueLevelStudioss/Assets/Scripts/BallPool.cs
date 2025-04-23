using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public GameObject ballPrefab;
    public int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            pool.Add(ball);
        }
    }

    public GameObject GetBall(Vector3 position, Transform paddle)
    {
        foreach (var ball in pool)
        {
            if (!ball.activeInHierarchy)
            {
                ball.transform.position = position;
                ball.SetActive(true);

                var bc = ball.GetComponent<BallController>();
                bc.paddle = paddle;

                return ball;
            }
        }

        // Si no hay pelotas disponibles, expandimos el pool
        GameObject newBall = Instantiate(ballPrefab, position, Quaternion.identity);
        newBall.SetActive(true);
        pool.Add(newBall);

        var newBC = newBall.GetComponent<BallController>();
        newBC.paddle = paddle;

        return newBall;
    }
}
