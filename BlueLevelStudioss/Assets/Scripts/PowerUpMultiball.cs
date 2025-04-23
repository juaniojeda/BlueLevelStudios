using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMultiball : MonoBehaviour, ICustomUpdate
{
    public float fallSpeed = 2f;
    public Transform paddle;
    public GameObject ballPrefab;
    public BallPool ballPool;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        UpdateManager.Instance.Register(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        _transform.position += Vector3.down * fallSpeed * deltaTime;

        CheckCollisionWithPaddle();
    }

    void CheckCollisionWithPaddle()
    {
        Vector3 pos = _transform.position;
        Vector3 paddlePos = paddle.position;

        float paddleWidth = 2.5f;
        float paddleHeight = 0.5f;

        if (pos.x > paddlePos.x - paddleWidth / 2 &&
            pos.x < paddlePos.x + paddleWidth / 2 &&
            pos.y > paddlePos.y - paddleHeight / 2 &&
            pos.y < paddlePos.y + paddleHeight / 2)
        {
            ActivatePowerUp();
        }
    }

    void ActivatePowerUp()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newBall = ballPool.GetBall(paddle.position + Vector3.up * 0.6f, paddle);
            BallController bc = newBall.GetComponent<BallController>();
            bc.isMainBall = false;
            bc.LaunchBall();
        }

        Destroy(gameObject); // Eliminar power-up
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
