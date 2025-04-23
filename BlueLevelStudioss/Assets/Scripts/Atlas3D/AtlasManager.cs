using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AtlasManager
{
    private static Material _sharedAtlasMaterial;
    private static AtlasSO _atlasSO;

    public static Material GetSharedAtlasMaterial(AtlasSO atlasSO)
    {
        if (_atlasSO != atlasSO || _sharedAtlasMaterial == null)
        {
            _atlasSO = atlasSO;
            _sharedAtlasMaterial = atlasSO.sharedMaterial;
        }

        return _sharedAtlasMaterial;
    }
}