using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshRenderer))]
public class MaterialSwapper : MonoBehaviour {

    public int materialSetIndex;

    private MeshRenderer meshrenderer;
    // Use this for initialization
    void Start () {
        meshrenderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMaterial(Material m)
    {
        if (meshrenderer == null)
        {
            meshrenderer = GetComponent<MeshRenderer>();
        }
        meshrenderer.material = m;
    }
}
