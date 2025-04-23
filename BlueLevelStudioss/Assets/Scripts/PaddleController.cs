using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour, ICustomUpdate
{
    [SerializeField] private float speed = 5f;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        UpdateManager.Instance.Register(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        float input = Input.GetAxisRaw("Horizontal"); // A/D o Flechas
        Vector3 movement = new Vector3(input * speed * deltaTime, 0f, 0f);
        _transform.position += movement;
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
