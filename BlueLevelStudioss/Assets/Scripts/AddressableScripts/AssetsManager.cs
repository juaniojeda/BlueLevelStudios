using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

[DefaultExecutionOrder(-200)]
public class AssetsManager : MonoBehaviour
{
    public static AssetsManager Instance { get; private set; }

    [Header("Addressable References")]
    [Tooltip("Arrastra aquí todos los AssetReferences que quieras cargar")]
    [SerializeField] private List<AssetReference> assetReferences;

    [Header("Remote Settings (Opcional)")]
    [Tooltip("Si está activado, redirige bundles a cloudURL en lugar de localURL.")]
    [SerializeField] private bool useRemoteAssets = true;
    [SerializeField] private string localURL = "http://localhost:3000/";
    [SerializeField] private string cloudURL = "https://myserver.com/";

    [Header("Levels")]
    [Tooltip("Arrastra aquí los AssetReferences de tus prefabs de nivel")]
    [SerializeField] private List<AssetReference> levelReferences;

    // Evento que se dispara cuando todos los assets han cargado
    public event Action OnLoadComplete;

    // Diccionario interno de assets cargados
    private Dictionary<string, GameObject> loadedAssets = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // Singleton + persistencia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configuración de remoto (opcional)
        if (useRemoteAssets)
            Addressables.ResourceManager.InternalIdTransformFunc += ChangeAssetUrlToPrivateServer;

        // Iniciar carga de assets
        StartCoroutine(LoadAssetsCoroutine());
    }

    private IEnumerator LoadAssetsCoroutine()
    {
        // 1) Concatenamos ambas listas en una sola
        var allRefs = assetReferences.Concat(levelReferences).ToList();          
        int total = allRefs.Count;                                              
        int loaded = 0;

        // 2) Iteramos sobre todos los AssetReferences (incluyendo niveles)
        foreach (var reference in allRefs)
        {
            var handle = reference.LoadAssetAsync<GameObject>();
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 3) Usamos el nombre del prefab como clave ("Nivel1", "Brick", etc.)
                string key = handle.Result.name;                                   
                loadedAssets[key] = handle.Result;
                loaded++;
            }
            else
            {
                Debug.LogError($"AssetsManager: Error cargando '{reference.RuntimeKey}': {handle.OperationException}");
            }
        }

        // 4) Solo disparamos OnLoadComplete cuando hayamos cargado todo
        if (loaded == total)                                                     
            OnLoadComplete?.Invoke();
    }

    private string ChangeAssetUrlToPrivateServer(IResourceLocation location)
    {
        string url = location.InternalId;
        if (url.StartsWith(localURL, StringComparison.OrdinalIgnoreCase))
            url = url.Replace(localURL, cloudURL);
        return url;
    }

    /// <summary>
    /// Suscribe un callback al evento de carga completa de assets.
    /// </summary>
    public void SubscribeOnLoadComplete(Action callback)
    {
        OnLoadComplete += callback;
    }

    /// <summary>
    /// Instancia un prefab ya cargado por su nombre (key).
    /// </summary>
    public GameObject GetInstance(string assetName)
    {
        if (loadedAssets.TryGetValue(assetName, out var prefab))
            return Instantiate(prefab);

        Debug.LogError($"AssetsManager: Asset '{assetName}' no encontrado.");
        return null;
    }
}
