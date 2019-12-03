using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSwapper : MonoBehaviour
{
    // name of swapper for first material
    public string swapperName;
    // name of swapper from second - last material, if empty use what's in mesh
    public string[] subSwapperName;
    public int AdditionalIndex = -1;

    private MeshRenderer _meshrenderer;

    public MeshRenderer MeshRenderer
    {
        get
        {
            if (!_meshrenderer)
            {
                _meshrenderer = GetComponent<MeshRenderer>();
            }
            return _meshrenderer;
        }
    }

    public void SetMaterial(Material m)
    {
        MeshRenderer.material = m;
        if (AdditionalIndex >= 0)
        {
            Material[] matArray = MeshRenderer.materials;
            matArray[AdditionalIndex] = m;
            MeshRenderer.materials = matArray;
        }
    }
}
