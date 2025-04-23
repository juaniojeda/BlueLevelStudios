using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AtlasApplier : MonoBehaviour
{
    public AtlasSO atlasMaster;
    public Rect uvRect; // UV en coordenadas de tiles. Ej: (0,0)-(0.25,0.25) para 4x4

    void Start()
    {
        ApplyAtlasMaterial();
    }

    void ApplyAtlasMaterial()
    {
        Renderer objectRenderer = GetComponent<Renderer>();

        // 1. Aplica el material compartido
        Material sharedMaterial = AtlasManager.GetSharedAtlasMaterial(atlasMaster);
        if (objectRenderer.sharedMaterial != sharedMaterial)
            objectRenderer.sharedMaterial = sharedMaterial;

        // 2. Aplica UV con MaterialPropertyBlock
        Vector2 scale = new Vector2(
            uvRect.width / atlasMaster.atlasTextureSize.x,
            uvRect.height / atlasMaster.atlasTextureSize.y
        );

        Vector2 offset = new Vector2(
            uvRect.x / atlasMaster.atlasTextureSize.x,
            uvRect.y / atlasMaster.atlasTextureSize.y
        );

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        objectRenderer.GetPropertyBlock(block);
        block.SetVector("_MainTex_ST", new Vector4(scale.x, scale.y, offset.x, offset.y));
        objectRenderer.SetPropertyBlock(block);
    }
}
