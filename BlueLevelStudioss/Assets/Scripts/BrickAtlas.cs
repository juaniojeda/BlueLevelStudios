using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BrickAtlas : MonoBehaviour
{
    [Header("Atlas Settings")]
    public int atlasColumns = 4; // N?mero de columnas en el atlas
    public int atlasRows = 4;    // N?mero de filas en el atlas

    private static readonly int AtlasOffsetID = Shader.PropertyToID("_AtlasOffset");
    private static readonly int AtlasTilingID = Shader.PropertyToID("_AtlasTiling");

    private void Awake()
    {
        ApplyUniqueTextureFromAtlas();
    }

    private void ApplyUniqueTextureFromAtlas()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (!renderer) return;

        // Elegimos un ?ndice aleatorio del atlas (puede cambiar por l?gica de nivel)
        int totalTiles = atlasColumns * atlasRows;
        int index = Random.Range(0, totalTiles);

        // Calculamos la posici?n en la grilla
        int x = index % atlasColumns;
        int y = atlasRows - 1 - (index / atlasColumns); // Invertimos eje Y

        Vector2 tiling = new Vector2(1f / atlasColumns, 1f / atlasRows);
        Vector2 offset = new Vector2(x * tiling.x, y * tiling.y);

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetVector(AtlasTilingID, tiling);
        mpb.SetVector(AtlasOffsetID, offset);

        renderer.SetPropertyBlock(mpb);
    }
}