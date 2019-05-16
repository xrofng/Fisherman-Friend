using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MaterialSwapIdentifier
{
    public string swapName;
    public Material[] resultMat = new Material[8];
}

public class MaterialManager : PersistentSingleton<MaterialManager>
{
    [SerializeField]
    public List<MaterialSwapIdentifier> materialSwaps = new List<MaterialSwapIdentifier>();
    // Cache of materialSwaps
    // Key is name of material will be swapped
    public Dictionary<string, MaterialSwapIdentifier> cacheMaterialSwap = new Dictionary<string, MaterialSwapIdentifier>();

    protected override void Awake()
    {
        base.Awake();
        foreach (MaterialSwapIdentifier identifier in materialSwaps)
        {
            if (cacheMaterialSwap.ContainsKey(identifier.swapName))
            {
                Debug.LogWarning("2 or more swap material used");
            }
            else
            {
                cacheMaterialSwap.Add(identifier.swapName, identifier);
            }
        }
    }

    public GameObject InstantiatePlayer(GameObject go, int colorIndex)
    {
        GameObject create = Instantiate(go);

        create.SetActive(true);

        if (go.GetComponent<Player>())
        {
            return GetChangedColorPlayer(create, colorIndex);
        }
        return null;

    }

    public GameObject GetChangedColorPlayer(GameObject player, int colorIndex)
    {
        MaterialSwapper[] mSwap;
        mSwap = player.GetComponentsInChildren<MaterialSwapper>();

        if (mSwap != null)
        {
            ChangeMaterial(mSwap, colorIndex);
        }
        else
        {
            // Try again, looking for inactive GameObjects
            MaterialSwapper[] mSwapInactive = GetComponentsInChildren<MaterialSwapper>(true);
            ChangeMaterial(mSwapInactive, colorIndex);
        }
        return player;
    }

    void ChangeMaterial(MaterialSwapper[] mSwap, int colorIndex)
    {
        foreach (MaterialSwapper m in mSwap)
        {
            if (cacheMaterialSwap.ContainsKey(m.swapperName))
            {
                Material[] subMats = null;

                if (m.subSwapperName.Length > 0)
                {
                    subMats = GetSubSwapperMaterials(m.subSwapperName,colorIndex);
                }

                m.SetMaterial(GetTargetMaterial(m.swapperName, colorIndex), subMats);
            }

        }
    }

    /// <summary>
    /// evaluate material array for second-last materials
    /// </summary>
    /// <param name="subSwapperName"></param>
    /// <param name="colorIndex"></param>
    /// <returns></returns>
    Material[] GetSubSwapperMaterials(string[] subSwapperName, int colorIndex)
    {
        List<Material> subMats = new List<Material>();
        for (int i = 0; i < subSwapperName.Length; i++)
        {
            if (cacheMaterialSwap.ContainsKey(subSwapperName[i]))
            {
                subMats.Add(GetTargetMaterial(subSwapperName[i], colorIndex));
            }else
            {
                subMats.Add(null);
            }
        }
        return subMats.ToArray();
    }


    Material GetTargetMaterial(string swapperName, int colorIndex)
    {
        return cacheMaterialSwap[swapperName].resultMat[colorIndex];
    }
}