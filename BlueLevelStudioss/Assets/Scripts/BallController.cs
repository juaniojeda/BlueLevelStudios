using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour, ICustomUpdate
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform paddle;

    private Vector3 velocity;
    private bool isLaunched = false;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;

        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.Register(this);
        }

    }

    void CheckPaddleCollision()
    {
        // AABB simple
        Vector3 ballPos = _transform.position;
        Vector3 paddlePos = paddle.position;

        float paddleWidth = 2.5f; // ajustá esto según tu modelo
        float paddleHeight = 1.0f;

        if (ballPos.x > paddlePos.x - paddleWidth / 2 &&
            ballPos.x < paddlePos.x + paddleWidth / 2 &&
            ballPos.y > paddlePos.y - paddleHeight / 2 &&
            ballPos.y < paddlePos.y + paddleHeight / 2)
        {
            // Rebote: cambiar dirección
            velocity.y = Mathf.Abs(velocity.y);

            // Efecto de rebote lateral según dónde pegó
            float offset = ballPos.x - paddlePos.x;
            velocity.x += offset * 2f; // más offset, más rebote lateral
        }
    }

    public void CustomUpdate(float deltaTime)
    {
        _transform.position += velocity * deltaTime;
        CheckWallCollisions();
        CheckPaddleCollision();
        if (!isLaunched)
        {
            // Seguir a la paleta
            _transform.position = paddle.position + new Vector3(0f, 0.5f, 0f);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchBall();
            }
            return;
        }

        _transform.position += velocity * deltaTime;

        CheckWallCollisions();
    }

    void LaunchBall()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        velocity = new Vector3(dir.x, dir.y, 0f) * speed;
        isLaunched = true;
    }

    void CheckWallCollisions()
    {
        Vector3 pos = _transform.position;

        // Rebote contra paredes horizontales
        if (Mathf.Abs(pos.x) >= 8f) velocity.x *= -1;

        // Rebote contra techo
        if (pos.y >= 4.5f) velocity.y *= -1;

        // Si se cae, resetear
        if (pos.y <= -4f)
        {
            isLaunched = false;
        }
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
