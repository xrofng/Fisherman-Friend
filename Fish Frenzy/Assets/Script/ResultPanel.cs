using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [HideInInspector]
    public RectTransform myRect;

    public Image panel;
    public Image rank;
    
    public Text[] ko = new Text[3];
    public Text[] death = new Text[3];
    public Text[] stage = new Text[3];

    void Start () {
        myRect = GetComponent<RectTransform>();

    }
	
	void Update () {
		
	}
}
