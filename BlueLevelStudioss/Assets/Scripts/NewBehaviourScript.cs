using UnityEngine;

public class BrickAtlasLogic
{
    private static readonly int AtlasOffsetID = Shader.PropertyToID("_AtlasOffset");
    private static readonly int AtlasTilingID = Shader.PropertyToID("_AtlasTiling");

    private float atlasColumns;
    private float atlasRows;
    private float index;

    public BrickAtlasLogic(float columns, float rows, float index = 0)
    {
        this.atlasColumns = columns;
        this.atlasRows = rows;
        this.index = index;
    }

    public void Apply(Renderer renderer)
    {
        if (renderer == null) return;

        float totalTiles = atlasColumns * atlasRows;
        index = Mathf.Clamp(index, 0, totalTiles - 1);

        float x = index % atlasColumns;
        float y = atlasRows - 1 - (index / atlasColumns);

        Vector2 tiling = new Vector2(1f / atlasColumns, 1f / atlasRows);
        Vector2 offset = new Vector2(x * tiling.x, y * tiling.y);

#if UNITY_EDITOR
        // Prevenir modificación de materiales de paquete
        string path = UnityEditor.AssetDatabase.GetAssetPath(renderer.sharedMaterial);
        if (path.StartsWith("Packages/"))
        {
            Debug.LogWarning($"? Material de solo lectura ({path}). Asigná un material propio en Assets.");
            return;
        }

        renderer.sharedMaterial.mainTextureScale = tiling;
        renderer.sharedMaterial.mainTextureOffset = offset;
#else
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetVector(AtlasTilingID, tiling);
        mpb.SetVector(AtlasOffsetID, offset);
        renderer.SetPropertyBlock(mpb);
#endif
    }
}
