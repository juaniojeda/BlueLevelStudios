using UnityEngine;
using YourGame.Utilities;

public class PaddleController : ICustomUpdate
{
    private Transform _transform;
    private float _speed;
    private float _halfWidth;
    private float _xLimit;

    public PaddleController(Transform transform, PaddleData data)
    {
        _transform = transform;
        ResetWithData(data);
    }

    /// <summary>
    /// Llama esto cada vez que cambies de nivel, en lugar de crear un PaddleController nuevo.
    /// </summary>
    public void ResetWithData(PaddleData data)
    {
        _speed = data.speed;
        _halfWidth = data.width * 0.5f;
        _xLimit = data.xLimit;
        // Si la escala de tu pala depende de data.width/height:
        //_transform.localScale = new Vector3(data.width, data.height, 1f);
    }

    public void CustomUpdate(float deltaTime)
    {
        float move = Input.GetAxis("Horizontal") * _speed * deltaTime;
        Vector3 pos = _transform.position + Vector3.right * move;
        pos.x = Mathf.Clamp(pos.x, -_xLimit + _halfWidth, _xLimit - _halfWidth);
        _transform.position = pos;
    }
}
//using UnityEngine;
//using YourGame.Utilities;

//public class PaddleController : ICustomUpdate, IDisposable
//{
//    private readonly Transform _transform;
//    private readonly float _speed;
//    private readonly float _halfWidth;
//    private readonly float _xLimit;

//    public PaddleController(Transform transform, PaddleData data)
//    {
//        _transform = transform;
//        _speed = data.speed;
//        _halfWidth = data.width * 0.5f;
//        _xLimit = data.xLimit;
//        GameManager.Instance.Register(this);
//    }

//    public void CustomUpdate(float deltaTime)
//    {
//        // Lectura de entrada horizontal
//        float move = Input.GetAxis("Horizontal") * _speed * deltaTime;
//        Vector3 pos = _transform.position + Vector3.right * move;

//        // Clamp con el medio ancho para no salirse
//        pos.x = Mathf.Clamp(pos.x, -_xLimit + _halfWidth, _xLimit - _halfWidth);
//        _transform.position = pos;
//    }

//    public void Dispose()
//    {
//        GameManager.Instance.Unregister(this);
//    }
//}
