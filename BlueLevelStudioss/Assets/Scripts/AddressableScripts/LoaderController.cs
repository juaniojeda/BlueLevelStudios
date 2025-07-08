using UnityEngine;

public class LoaderController
{
    private readonly BallPool _ballPool;
    private readonly Vector3 _spawnPosition;
    private readonly Vector3 _initialDirection;
    private Transform paddleTransform;
    private PaddleData paddleData;
    internal PaddleController paddleController;


    /// <summary>
    /// Constructor puro.  
    /// - ballPool: tu pool de bolas  
    /// - spawnPosition: dónde aparecerá la bola  
    /// - initialDirection: dirección en la que sale la bola principal  
    /// </summary>
    public LoaderController(BallPool ballPool, Vector3 spawnPosition, Vector3 initialDirection)
    {
        _ballPool = ballPool;
        _spawnPosition = spawnPosition;
        _initialDirection = initialDirection;

        // Nos suscribimos al evento de carga de Addressables
        AssetsManager.Instance.SubscribeOnLoadComplete(OnAssetsLoaded);
    }

    private void OnAssetsLoaded()
    {
        // 1) Sacamos la bola del pool
        GameObject ballGO = _ballPool.GetBall(_spawnPosition);
        if (ballGO == null)
        {
            Debug.LogError("LoaderController: no pudo obtener bola del pool");
            return;
        }

        // 2) Obtenemos el controlador y la lanzamos
        var controller = ballGO.GetComponent<BallController>();
        if (controller != null)
            controller.LaunchBall(_initialDirection);
        else
            Debug.LogWarning("LoaderController: la bola no tiene BallController");

        GameObject paddleGO = AssetsManager.Instance.GetInstance("Paleta");
        if (paddleGO == null)
        {
            Debug.LogError("GameManager: no encontró el prefab 'Paleta'");
            return;
        }
        // Actualiza el transform que usará PaddleController
        paddleTransform = paddleGO.transform;

        // Luego configuras el controller:
        paddleController = new PaddleController(paddleTransform, paddleData);



    }
}
