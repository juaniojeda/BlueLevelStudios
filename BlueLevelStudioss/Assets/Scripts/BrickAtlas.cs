using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Renderer))]
public class BrickAtlas : MonoBehaviour
{
    [Header("Atlas Settings")]
    public int atlasColumns = 4;
    public int atlasRows = 4;
    public int index = 0; // Índice manual de la celda del atlas

    private static readonly int AtlasOffsetID = Shader.PropertyToID("_AtlasOffset");
    private static readonly int AtlasTilingID = Shader.PropertyToID("_AtlasTiling");

    private void Awake()
    {
        ApplyUniqueTextureFromAtlas();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyUniqueTextureFromAtlas();
    }
#endif

    private void ApplyUniqueTextureFromAtlas()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (!renderer) return;

        int totalTiles = atlasColumns * atlasRows;
        index = Mathf.Clamp(index, 0, totalTiles - 1);

        int x = index % atlasColumns;
        int y = atlasRows - 1 - (index / atlasColumns);

        Vector2 tiling = new Vector2(1f / atlasColumns, 1f / atlasRows);
        Vector2 offset = new Vector2(x * tiling.x, y * tiling.y);

#if UNITY_EDITOR
        // Prevención: NO modificar materiales de los packages
        string path = AssetDatabase.GetAssetPath(renderer.sharedMaterial);
        if (path.StartsWith("Packages/"))
        {
            Debug.LogWarning($"⚠ Estás intentando modificar un material de solo lectura ({path}). Asigná un material propio en Assets.");
            return;
        }

        // Modificación visible en editor
        renderer.sharedMaterial.mainTextureScale = tiling;
        renderer.sharedMaterial.mainTextureOffset = offset;
#else
        // En runtime: optimización con property block
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetVector(AtlasTilingID, tiling);
        mpb.SetVector(AtlasOffsetID, offset);
        renderer.SetPropertyBlock(mpb);
#endif
    }
}