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
    [SerializeField] public bool isMainBall = true;
    private Collider myCollider;

    private Vector3 velocity;
    private bool isLaunched = false;
    private bool initialized = false;

    private Brick[] bricks;

    private Transform _transform;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        _transform = transform;

        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.Register(this);
        }

    }

    void CheckPaddleCollision()
    {
        Vector3 ballPos = _transform.position;
        Vector3 paddlePos = paddle.position;

        float paddleWidth = 4.0f;
        float paddleHeight = 1.0f;

        if (ballPos.x > paddlePos.x - paddleWidth / 2 &&
            ballPos.x < paddlePos.x + paddleWidth / 2 &&
            ballPos.y > paddlePos.y - paddleHeight / 2 &&
            ballPos.y < paddlePos.y + paddleHeight / 2)
        {
            // Calculamos nueva direcci?n
            float offset = (ballPos.x - paddlePos.x) / (paddleWidth / 2f); // -1 a 1
            offset = Mathf.Clamp(offset, -1f, 1f); // por si acaso

            Vector2 newDir = new Vector2(offset, 1).normalized;
            velocity = new Vector3(newDir.x, newDir.y, 0f) * speed;
        }
    }


    public void CustomUpdate(float deltaTime)
    {
        if (!gameObject.activeInHierarchy) return;
        _transform.position += velocity * deltaTime;
        CheckWallCollisions();
        CheckPaddleCollision();
        if (!isLaunched)
        {
            if (isMainBall)
            {
                _transform.position = paddle.position + new Vector3(0f, 0.5f, 0f);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    LaunchBall();
                }
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

        // Rebote contra bordes izquierdo y derecho
        if (pos.x <= leftLimit)
        {
            pos.x = leftLimit;
            velocity.x = Mathf.Abs(velocity.x);
        }
        else if (pos.x >= rightLimit)
        {
            pos.x = rightLimit;
            velocity.x = -Mathf.Abs(velocity.x);
        }

        // Rebote contra el techo
        if (pos.y >= topLimit)
        {
            pos.y = topLimit;
            velocity.y = -Mathf.Abs(velocity.y);
        }

        // Fondo
        if (pos.y <= bottomLimit)
        {
            if (isMainBall)
            {
                isLaunched = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
            return;
        }

        _transform.position = pos;
    }
    private void OnEnable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Register(this);

        if (myCollider != null)
            myCollider.enabled = true;

        isLaunched = false;
        velocity = Vector3.zero;
    }
    private void OnDisable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);

        if (myCollider != null)
            myCollider.enabled = false;
    }


    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
