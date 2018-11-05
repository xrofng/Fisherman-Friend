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

    protected int elementX;
    protected int elementY;

    protected Material targetMaterial
    {
        get
        {
            InitMaterialSetList();
            return materialSetList[elementX][elementY];
        }
    }

    public void InitMaterialSetList()
    {
        if(materialSetList.Count == 0)
        {
            materialSetList.Add(materialSet0);
            materialSetList.Add(materialSet1);
        }
      
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public GameObject InstantiatePlayer(GameObject go , int tempId)
    {
        GameObject create = Instantiate(go);
        
            create.SetActive(true);
        
        if (go.GetComponent<Player>())
        {
           
            MaterialSwapper[] mSwap;
            mSwap = create.GetComponentsInChildren<MaterialSwapper>();
            
            if (mSwap != null)
            {
                
                changeMaterial(mSwap,tempId);
                
            }
            else
            {
                // Try again, looking for inactive GameObjects
                MaterialSwapper[] mSwapInactive = GetComponentsInChildren<MaterialSwapper>(true);
                changeMaterial(mSwapInactive, tempId);
            }
            return create ;
        }
        return null;

    }

    void changeMaterial(MaterialSwapper[] mSwap,int subIndex)
    {
        foreach(MaterialSwapper m in mSwap)
        {
            elementX = m.materialSetIndex;
            elementY = subIndex;
            m.setMaterial(targetMaterial);
        }
    }
}
