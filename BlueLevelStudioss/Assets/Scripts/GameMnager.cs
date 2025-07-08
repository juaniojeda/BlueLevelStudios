using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using YourGame.Utilities;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour, ICustomUpdate, ITimeProvider
{
    public static GameManager Instance { get; private set; }
    private readonly List<ICustomUpdate> updatables = new();

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;

    [Header("Levels")]
    [Tooltip("Contenedor vacío donde instanciamos el nivel")]
    [SerializeField] private Transform levelRoot;

    [Header("Containers")]
    /*[SerializeField] private Transform nivelContainer;    */// donde instancio el prefab NivelX
    [SerializeField] private Transform bricksContainer;   // donde reuso/activo los bricks


    [Header("Level Flow")]
    [Tooltip("Cuántos niveles hay en total")]
    [SerializeField] private int totalLevels = 10;
    private int currentLevel;


    [Tooltip("Nivel con el que arrancamos (1-based)")]
    [SerializeField] private int startingLevel = 1;

    private LevelController levelController;

    [Header("Ball Settings (Scriptable)")]
    [SerializeField] private BallData ballData;
    //internal BallPool ballPool;
    private GameObject _currentBallGO;
    private BallPool ballPool;
    private BallController _mainBallController;

    [Header("Bricks")]
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int poolSize = 30;
    [SerializeField] private BrickGeneratorConfig generatorConfig;
    [SerializeField] private Transform brickStartPosition;
    private BrickGenerator brickGenerator;
    private BrickPool brickPool;

    [Header("Atlas Settings")]
    [SerializeField] private float atlasColumns = 4;
    [SerializeField] private float atlasRows = 4;
    [SerializeField] private bool randomizeIndex = true;
    [SerializeField] private float fixedIndex = 0f;

    [Header("Paddle")]
    [SerializeField] private Transform paddleTransform;
    [SerializeField] private PaddleData paddleData;
    internal PaddleController paddleController;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TextMeshProUGUI timerText;
    private UITimerDisplay uiTimerDisplay;

    [Header("PowerUp")]
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float powerUpFallSpeed = 2f;

    // Game state
    private bool gameEnded, loadingScene;
    private float timer, timeLimit = 120f, endDelay = 2f, endTimer;
    private string targetScene;

    private void Awake()
    {
        // Singleton + persistence
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AssetsManager.Instance.SubscribeOnLoadComplete(OnAssetsLoaded);

        //// 1) Generate bricks
        //var brickPool = new BrickPool(brickPrefab, poolSize);
        //brickGenerator = new BrickGenerator(brickPool, generatorConfig, brickStartPosition);
        //brickGenerator.GenerateBricks();

        //// 2) Prepare ball pool and launch main ball
        //ballPool = new BallPool(ballData.ballPrefab, ballData.poolSize);
        //var mainBall = SpawnBall(true, paddleTransform.position + Vector3.up * 0.6f);

        //// 3) Configure paddle from ScriptableObject
        //paddleController = new PaddleController(paddleTransform, paddleData);

        //// 4) UI setup
        //uiTimerDisplay = new UITimerDisplay(timerText, this);
        //uiManager.Initialize(this, mainBall);

        //// 5) Register for pure update loop
        //Register(this);

        //// 6) Apply atlas to each brick
        //var bricks = GameObject.FindGameObjectsWithTag("Brick");
        //foreach (var b in bricks)
        //{
        //    var renderer = b.GetComponent<Renderer>();
        //    if (renderer == null) continue;

        //    float indexToUse = randomizeIndex
        //        ? Random.Range(0, atlasColumns * atlasRows)
        //        : fixedIndex;

        //    var atlas = new BrickAtlasLogic(atlasColumns, atlasRows, indexToUse);
        //    atlas.Apply(renderer);
        //}
    }

    private void OnAssetsLoaded()
    {
        // 1) Instanciar nivel
        levelController = new LevelController(levelRoot);
        levelController.LoadLevel(startingLevel);


        currentLevel = startingLevel;

        // 2) **Crear una sola vez** el pool de bricks
        brickPool = new BrickPool(brickPrefab, generatorConfig.rows * generatorConfig.columns);
        brickGenerator = new BrickGenerator(brickPool, generatorConfig, brickStartPosition, bricksContainer);
        brickGenerator.GenerateBricks();

        // 3) **Crear una sola vez** el pool de bolas
        ballPool = new BallPool(ballData.ballPrefab, ballData.poolSize);

        ApplyAtlasToBricks();

        // 4) Lanzar la bola principal
        Vector3 spawnPos = paddleTransform.position + Vector3.up * 0.6f;
        _mainBallController = SpawnBall(true, spawnPos);
        _currentBallGO = _mainBallController.Transform.gameObject;

        //// 5) Configurar la pala
        //paddleController = new PaddleController(paddleTransform, paddleData);

        // 5) Crear y suscribir SOLO UNA VEZ el PaddleController
        paddleController = new PaddleController(paddleTransform, paddleData);
        Register(paddleController);

        // 6) Inicializar UI con el BallController de la bola
        uiManager.Initialize(this, _mainBallController);

        // 7) Registrar sólo una vez
        Register(this);
    }

    private BallController SpawnBall(bool isMainBall, Vector3 atPosition)
    {
        var go = Instantiate(ballData.ballPrefab, atPosition, Quaternion.identity);
        go.SetActive(true);

        var sphere = go.GetComponent<SphereCollider>();
        float radius = (sphere != null)
            ? sphere.radius * Mathf.Max(
                go.transform.localScale.x,
                go.transform.localScale.y,
                go.transform.localScale.z)
            : 0.5f;

        var audioCtrl = FindObjectOfType<GlobalAudioController>();
        return new BallController(
            go.transform,
            paddleTransform,
            radius,
            ballData.speed,
            ballData.leftLimit,
            ballData.rightLimit,
            ballData.topLimit,
            ballData.bottomLimit,
            audioCtrl,
            isMainBall
        );
    }

    public BallController SpawnExtraBall(Vector3 atPosition, Vector3 direction)
    {
        var extra = SpawnBall(false, atPosition);
        extra.LaunchBall(direction);
        return extra;
    }

    public void SpawnPowerUpMultiball(Vector3 atPosition)
    {
        var go = Instantiate(powerUpPrefab, atPosition, Quaternion.identity);
        new PowerUpMultiball(go.transform, paddleTransform, powerUpFallSpeed);
    }

    public void Register(ICustomUpdate obj) => updatables.Add(obj);
    public void Unregister(ICustomUpdate obj) => updatables.Remove(obj);

    private void LateUpdate()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < updatables.Count; i++)
            updatables[i]?.CustomUpdate(dt);
    }

    public void CustomUpdate(float deltaTime)
    {
        // 1) Si estamos en transición de escena (victoria/fallo), esperamos…
        if (loadingScene)
        {
            endTimer += deltaTime;
            if (endTimer >= endDelay)
            {
                Unregister(this);
                SceneManager.LoadScene(targetScene);
            }
            return;
        }

        // 2) Si ya estamos en gameEnded, no hacemos nada
        if (gameEnded)
            return;

        // 3) Cada frame contamos tiempo
        timer += deltaTime;

        // 4) Si se acabaron los bricks, avanzamos de nivel o ganamos el juego
        if (AllBricksDestroyed())
        {
            // Si aún quedan niveles pendientes
            if (currentLevel < totalLevels)
            {
                currentLevel++;
                StartNextLevel();
            }
            else
            {
                // Era el último nivel
                gameEnded = true;
                Debug.Log("¡GANASTE TODOS LOS NIVELES!");
                LoadSceneWithDelay("VictoryScreen");
            }
        }
        // 5) Si se agota el tiempo antes de romperlos todos, game over
        else if (timer >= timeLimit)
        {
            gameEnded = true;
            Debug.Log("¡PERDISTE POR TIEMPO!");
            LoadSceneWithDelay("GameOverScreen");
        }
    }

    private void StartNextLevel()
    {
        
        // 2) Cargar siguiente nivel
        levelController.LoadLevel(currentLevel);

        // 3) Resetear y regenerar bricks
        brickPool.ReturnAllBricks();
        brickGenerator.GenerateBricks();

        ApplyAtlasToBricks();

        // Destruye todos los power-ups activos
        foreach (var pu in GameObject.FindGameObjectsWithTag("PowerUp"))
            Destroy(pu);

        // Destruye todas las bolas activas (principal + extras)
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
            Destroy(ball);

        // 4) Destruir la bola anterior
        if (_currentBallGO != null) Destroy(_currentBallGO);

        // 5) Reutilizar el pool de bolas
        var spawnPos = paddleTransform.position + Vector3.up * 0.6f;          
        _mainBallController = SpawnBall(true, spawnPos);
        _currentBallGO = _mainBallController.Transform.gameObject;

        //// 6) Reconfigurar la pala si hace falta
        //paddleController = new PaddleController(paddleTransform, paddleData);
        // 6) En lugar de recrear el controller, reseteamos sus datos:
        paddleController.ResetWithData(paddleData);
        var pr = paddleTransform.GetComponent<Renderer>();
        if (pr != null)
        {
            // Usamos el mismo atlasSettings que para los bricks:
            float idx = randomizeIndex
                ? Random.Range(0, atlasColumns * atlasRows)
                : fixedIndex;
            var atlas = new BrickAtlasLogic(atlasColumns, atlasRows, idx);
            atlas.Apply(pr);
        }
        // 7) Reset timer / estados
        timer = 0f;
        gameEnded = false;

        // 8) UI: reinicializar con el nuevo BallController
        uiManager.Initialize(this, _mainBallController);

        // NO te vuelvas a registrar aquí
    }
    public float GetTimeRemaining() => Mathf.Max(0f, timeLimit - timer);

    private bool AllBricksDestroyed()
    {
        var bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (var b in bricks)
            if (b.activeInHierarchy)
                return false;
        return true;
    }

    private void LoadSceneWithDelay(string scene)
    {
        loadingScene = true;
        targetScene = scene;
        endTimer = 0f;
    }

    private void OnDestroy() => Unregister(this);

    public void HandleBrickHit(GameObject brickGO)
    {
        GlobalAudioController.Instance.PlayBrickBreak();
        brickGO.SetActive(false);
        if (Random.value < 0.05f)
            SpawnPowerUpMultiball(brickGO.transform.position);
    }

    private void ApplyAtlasToBricks()
    {
        // Todas las mallas bajo bricksContainer
        var renderers = bricksContainer.GetComponentsInChildren<Renderer>(includeInactive: false);
        float count = atlasColumns * atlasRows;

        foreach (var r in renderers)
        {
            // El índice puede ser fijo o aleatorio
            float idx = randomizeIndex
                ? Random.Range(0, count)
                : fixedIndex;

            var atlas = new BrickAtlasLogic(atlasColumns, atlasRows, idx);
            atlas.Apply(r);
        }
    }

}