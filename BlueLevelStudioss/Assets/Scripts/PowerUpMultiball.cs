using UnityEngine;
using YourGame.Utilities;

public class PowerUpMultiball : ICustomUpdate, IDisposable
{
    readonly Transform _transform;
    readonly Transform _paddle;
    readonly float _fallSpeed;

    public PowerUpMultiball(Transform transform, Transform paddle, float fallSpeed)
    {
        _transform = transform;
        _paddle = paddle;
        _fallSpeed = fallSpeed;
        GameManager.Instance.Register(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        if (_transform == null) { Dispose(); return; }

        // Caída vertical
        _transform.position += Vector3.down * _fallSpeed * deltaTime;

        // Detección AABB vs. círculo contra la paleta
        Vector2 pu = _transform.position;
        Vector2 p = _paddle.position;
        const float w = 4f, h = 0.5f;

        if (pu.x >= p.x - w / 2f && pu.x <= p.x + w / 2f &&
            pu.y >= p.y - h / 2f && pu.y <= p.y + h / 2f)
        {
            SpawnMultiball();
        }
    }

    private void SpawnMultiball()
    {
        const int count = 2;
        const float spreadAngle = 45f;  // abanico de ±22.5°
        Vector3 basePos = _paddle.position + Vector3.up * 0.6f;

        for (int i = 0; i < count; i++)
        {
            // Calcula ángulo equidistante: -spread/2  … +spread/2
            float angleDeg = -spreadAngle / 2f + spreadAngle * (i / (count - 1f));
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angleRad), Mathf.Cos(angleRad), 0f);

            // Lanza la bola con dirección en abanico
            GameManager.Instance.SpawnExtraBall(basePos, dir);
        }

        Dispose();
    }

    public void Dispose()
    {
        GameManager.Instance.Unregister(this);
        if (_transform != null)
            GameObject.Destroy(_transform.gameObject);
    }
}
