using System.Collections.Generic;
using UnityEngine;
using YourGame.Utilities;  // Aquí defines ICustomUpdate e IDisposable

public class BallController : ICustomUpdate, IDisposable
{
    private readonly Transform _transform;
    private readonly Transform _paddle;
    private readonly float _collisionRadius;
    private readonly float _speed;
    private readonly float _leftLimit, _rightLimit, _topLimit, _bottomLimit;
    private readonly GlobalAudioController _audioController;

    private Vector3 _velocity = Vector3.zero;
    private bool _isMainBall;
    private bool _isLaunched;

    public int TotalPaddleBounces { get; private set; }
    public Transform Transform => _transform;

    public BallController(
        Transform ballTransform,
        Transform paddleTransform,
        float collisionRadius,
        float speed,
        float left, float right, float top, float bottom,
        GlobalAudioController audioController,
        bool isMainBall = true)
    {
        _transform = ballTransform;
        _paddle = paddleTransform;
        _collisionRadius = collisionRadius;
        _speed = speed;
        _leftLimit = left;
        _rightLimit = right;
        _topLimit = top;
        _bottomLimit = bottom;
        _audioController = audioController;
        _isMainBall = isMainBall;

        TotalPaddleBounces = 0;
        GameManager.Instance.Register(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        if (_transform == null || !_transform.gameObject.activeInHierarchy)
        {
            Dispose();
            return;
        }

        // Antes de lanzar, la bola sigue a la paleta
        if (!_isLaunched)
        {
            _transform.position = _paddle.position + Vector3.up * 0.6f;
            if (_isMainBall && Input.GetKeyDown(KeyCode.Space))
                LaunchBall();  // usa la sobrecarga sin parámetros
            return;
        }

        // Movimiento
        _transform.position += _velocity * deltaTime;

        // Colisiones con paredes y paleta
        CheckWallCollisions();
        CheckPaddleCollision();

        // Colisión manual con ladrillos (AABB vs círculo)
        CheckBrickCollisions();

        // Fuera de límite inferior
        if (_transform.position.y <= _bottomLimit)
        {
            if (_isMainBall)
            {
                ResetOnPaddle();
                LifeSystemAccess.LoseLife();
            }
            else
            {
                _transform.gameObject.SetActive(false);
                Dispose();
            }
        }
    }

    private void CheckWallCollisions()
    {
        Vector3 pos = _transform.position;
        bool bounced = false;

        if (pos.x <= _leftLimit) { pos.x = _leftLimit; _velocity.x = Mathf.Abs(_velocity.x); bounced = true; }
        if (pos.x >= _rightLimit) { pos.x = _rightLimit; _velocity.x = -Mathf.Abs(_velocity.x); bounced = true; }
        if (pos.y >= _topLimit) { pos.y = _topLimit; _velocity.y = -Mathf.Abs(_velocity.y); bounced = true; }

        if (bounced)
            _audioController?.PlayWallBounce();

        _transform.position = pos;
    }

    private void CheckPaddleCollision()
    {
        Vector2 b = _transform.position;
        Vector2 p = _paddle.position;
        const float w = 4f, h = 1f;

        if (b.x >= p.x - w / 2f && b.x <= p.x + w / 2f &&
            b.y >= p.y - h / 2f && b.y <= p.y + h / 2f &&
            _velocity.y < 0f)
        {
            float offset = Mathf.Clamp((b.x - p.x) / (w / 2f), -1f, 1f);
            Vector2 dir = new Vector2(offset, 1f).normalized;
            _velocity = new Vector3(dir.x, dir.y, 0f) * _speed;
            _audioController?.PlayPaddleBounce();
            TotalPaddleBounces++;
        }
    }

    private void CheckBrickCollisions()
    {
        foreach (var brickGO in GameObject.FindGameObjectsWithTag("Brick"))
        {
            if (!brickGO.activeInHierarchy) continue;

            Transform t = brickGO.transform;
            Vector2 brickPos = t.position;
            Vector2 halfSize = new Vector2(t.localScale.x, t.localScale.y) * 0.5f;
            Vector2 ballPos2D = _transform.position;

            float closestX = Mathf.Clamp(ballPos2D.x, brickPos.x - halfSize.x, brickPos.x + halfSize.x);
            float closestY = Mathf.Clamp(ballPos2D.y, brickPos.y - halfSize.y, brickPos.y + halfSize.y);
            Vector2 diff = ballPos2D - new Vector2(closestX, closestY);

            if (diff.sqrMagnitude <= _collisionRadius * _collisionRadius)
            {
                Vector2 normal = Mathf.Abs(diff.x) > Mathf.Abs(diff.y)
                                 ? new Vector2(Mathf.Sign(diff.x), 0f)
                                 : new Vector2(0f, Mathf.Sign(diff.y));

                _velocity = Vector3.Reflect(_velocity, new Vector3(normal.x, normal.y, 0f))
                            .normalized * _speed;

                _transform.position = new Vector3(
                    closestX + normal.x * _collisionRadius,
                    closestY + normal.y * _collisionRadius,
                    _transform.position.z
                );

                GameManager.Instance.HandleBrickHit(brickGO);
                break;
            }
        }
    }

    /// <summary>
    /// Lanza la bola hacia arriba (módulo = speed).
    /// </summary>
    public void LaunchBall()
    {
        LaunchBall(Vector3.up);
    }

    /// <summary>
    /// Lanza la bola en la dirección dada (módulo = speed).
    /// </summary>
    public void LaunchBall(Vector3 direction)
    {
        _isLaunched = true;
        _velocity = direction.normalized * _speed;
    }

    public void ResetOnPaddle()
    {
        _isLaunched = false;
        _velocity = Vector3.zero;
        _transform.position = _paddle.position + Vector3.up * 0.6f;
    }

    public void Dispose()
    {
        GameManager.Instance.Unregister(this);
    }
}
