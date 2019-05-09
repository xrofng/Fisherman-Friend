using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnvironmentInteraction : PlayerAbility
{
    public GameObject coastHolder;
    private List<Transform> coastPoint = new List<Transform>();

    protected override void Initialization()
    {
        base.Initialization();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNearestCoast()
    {
        return portRoyal.coastPoints[0].gameObject;
    }

   


}
