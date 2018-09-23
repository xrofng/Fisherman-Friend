using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePercent : MonoBehaviour {
    public List<Text> text_percent;
    private PortRoyal portroyal;
    public int superRed_P = 300;
    public int halfYellow_P = 60;
    public int lowWhite = 0;
	// Use this for initialization
	void Start () {
        portroyal = FindObjectOfType<PortRoyal>();
	}
	
	// Update is called once per frame
	void Update () {
        updateDamagePerent();
    }

    void updateDamagePerent()
    {
        for(int i = 0; i < 4; i++)
        {
           
        }
    }

    Color colorScale(int percent, int from, int to)
    {
        float R = Mathf.SmoothStep(0, 300, percent/100);
        return new Color(R,0,0);
    }
}
