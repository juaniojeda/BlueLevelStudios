using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancedMaterialsScanner : MonoBehaviour
{
    [ContextMenu("Scan Scene Materials")]
    void ScanInstancedMaterials()
    {
        HashSet<Material> instancedMaterials = new HashSet<Material>();
        int materialCount = 0;

        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat != null && !instancedMaterials.Contains(mat))
                {
                    instancedMaterials.Add(mat);
                    materialCount++;
                }
            }
        }

        Debug.Log($"Scanned renderers: {renderers.Length}");
        Debug.Log($"Instanced materials found: {materialCount}");

        foreach (Material mat in instancedMaterials)
        {
            Debug.Log($"Instanced material: {mat.name}", mat);
        }
    }
}