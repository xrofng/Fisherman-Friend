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

    public GameObject InstantiatePlayer(GameObject go, int colorIndex, int hatIndex)
    {
        GameObject create = Instantiate(go);

        create.SetActive(true);

        if (go.GetComponent<Player>())
        {
            create = GetChangedHatPlayer(create.GetComponent<PlayerModel>(), hatIndex, 170);
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

    public GameObject GetChangedHatPlayer(PlayerModel playerModel, int hatIndex,float sizeMultiplier)
    {
        foreach(Transform child in playerModel.Hat.transform)
        {
            Destroy(child.gameObject);
        }

        List<Transform> newHatParts = new List<Transform>();
        GameObject newHat = Instantiate(PlayerData.Instance.hats[hatIndex].hat);
        foreach (Transform child in newHat.transform)
        {
            newHatParts.Add(child);
        }
        newHat.transform.localScale *= sizeMultiplier;
        newHat.gameObject.name = "new Hat";
        newHat.transform.parent = playerModel.Hat.transform.parent.transform;
        newHat.transform.position = playerModel.Hat.transform.position;
        foreach (Transform newHatPart in newHatParts)
        {
            newHatPart.parent = playerModel.Hat.transform;
        }
        Destroy(newHat.gameObject);

        return playerModel.gameObject;
    }


    void ChangeMaterial(MaterialSwapper[] mSwap, int colorIndex)
    {
        foreach (MaterialSwapper m in mSwap)
        {
            if (cacheMaterialSwap.ContainsKey(m.swapperName))
            {
                m.SetMaterial(GetTargetMaterial(m.swapperName, colorIndex));
            }

        }
    }

    Material GetTargetMaterial(string swapperName, int colorIndex)
    {
        return cacheMaterialSwap[swapperName].resultMat[colorIndex];
    }
}