using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteraction : MonoBehaviour
{
    public int damage;
    public int invicibilityFrame = 50;

    protected HitBoxStageInteraction hitBox;
    
    void Start () {
        hitBox = GetComponent<HitBoxStageInteraction>();
        hitBox.InvincibilityFrame = invicibilityFrame;
        hitBox.DamageCaused = damage;
        gameObject.layer = LayerMask.NameToLayer("StageInteraction");
    }
	
	void Update () {
		
	}


}
