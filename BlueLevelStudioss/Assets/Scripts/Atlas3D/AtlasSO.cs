using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAtlasSO", menuName = "Atlas/AtlasSO")]
public class AtlasSO : ScriptableObject
{
    public Material sharedMaterial;         // Material base del atlas
    public Vector2 atlasTextureSize = new Vector2(1, 1); // Tamaño del atlas en tiles (ej: 4x4)
}
