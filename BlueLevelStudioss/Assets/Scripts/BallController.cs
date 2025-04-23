using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour, ICustomUpdate
{
    [SerializeField] private float speed = 5f;
    [SerializeField] public Transform paddle;
    [SerializeField] private float leftLimit = -8f;
    [SerializeField] private float rightLimit = 8f;
    [SerializeField] private float topLimit = 4.5f;
    [SerializeField] private float bottomLimit = -4f;

    private Vector3 velocity;
    private bool isLaunched = false;
    private bool initialized = false;

    private Brick[] bricks;

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

        float paddleWidth = 4.0f; // ajustá esto según tu modelo
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

        if (!initialized)
        {
            bricks = bricks = Object.FindObjectsByType<Brick>(FindObjectsSortMode.None);
            initialized = true;
        }

        _transform.position += velocity * deltaTime;

        _transform.position += velocity * deltaTime;
        CheckWallCollisions();
        CheckPaddleCollision();
        CheckBrickCollisions();
    }

    void CheckBrickCollisions()
    {
        foreach (var brick in bricks)
        {
            if (brick == null || !brick.gameObject.activeInHierarchy) continue;

            Vector3 brickPos = brick.Position;
            Vector3 brickSize = brick.Size;
            Vector3 halfSize = brickSize * 0.5f;
            Vector3 ballPos = _transform.position;

            if (ballPos.x > brickPos.x - halfSize.x &&
                ballPos.x < brickPos.x + halfSize.x &&
                ballPos.y > brickPos.y - halfSize.y &&
                ballPos.y < brickPos.y + halfSize.y)
            {
                velocity.y *= -1;
                brick.DeactivateBrick();
                break;
            }
        }
    }

    public void LaunchBall()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        velocity = new Vector3(dir.x, dir.y, 0f) * speed;
        isLaunched = true;
    }

    void CheckWallCollisions()
    {
        Vector3 pos = _transform.position;

        // Rebote contra paredes horizontales
        if (pos.x <= leftLimit || pos.x >= rightLimit)
        {
            velocity.x *= -1;
            pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        }

        // Rebote contra techo
        if (pos.y >= topLimit)
        {
            velocity.y *= -1;
            pos.y = Mathf.Clamp(pos.y, -999f, topLimit); // solo limite superior
        }

        // Si se cae por debajo
        if (pos.y <= bottomLimit)
        {
            isLaunched = false;
        }

        _transform.position = pos;
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
