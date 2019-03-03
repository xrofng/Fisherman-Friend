using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : PersistentSingleton<MaterialManager>
{
    //direct by element X
    private List<Material[]> materialSetList = new List<Material[]>();
    //direct by element Y
    public Material[] materialSet0;
    public Material[] materialSet1;

    public GameObject InstantiatePlayer(GameObject go , int colorIndex)
    {
        GameObject create = Instantiate(go);
        
        create.SetActive(true);
        
        if (go.GetComponent<Player>())
        {
            return GetChangedColorPlayer(create, colorIndex);
        }
        return null;

    }

    public GameObject GetChangedColorPlayer(GameObject player , int colorIndex)
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

    void ChangeMaterial(MaterialSwapper[] mSwap,int colorIndex)
    {
        foreach(MaterialSwapper m in mSwap)
        {
            m.setMaterial(GetTargetMaterial(m.materialSetIndex , colorIndex));
        }
    }

    Material GetTargetMaterial(int setIndex,int colorIndex)
    {
        if (materialSetList.Count == 0)
        {
            materialSetList.Add(materialSet0);
            materialSetList.Add(materialSet1);
        }
        return materialSetList[setIndex][colorIndex];
    }
}
