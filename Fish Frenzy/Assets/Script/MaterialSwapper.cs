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

    public void SetMaterial(Material m,Material[] subMats)
    {
        if (subMats != null)
        {
            // mats size has to include first element
            Material[] mats = new Material[subMats.Length+1];
            for (int i=0;i<subMats.Length;i++)
            {
                // i+1 to assigne from second index
                if(subMats[i] != null)
                {
                    mats[i+1] = subMats[i];
                }else
                {
                    mats[i+1] = MeshRenderer.materials[i+1];
                }
            }
            MeshRenderer.materials = mats;
        }
        MeshRenderer.material = m;

    }
}
