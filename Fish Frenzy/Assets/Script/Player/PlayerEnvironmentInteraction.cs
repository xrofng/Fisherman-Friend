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
        //foreach (Transform child in coastHolder.transform)
        //{
        //    if (child != coastHolder.transform)
        //    {
        //        coastPoint.Add(child);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNearestCoast()
    {
        //
        return coastPoint[0].gameObject;
    }

    // Draw Path
    [Header("Debug")]
    public Color rayColor;
    public float wireSphereRadius = 1.0f;
    public bool showRay = true;
    void OnDrawGizmos()
    {
        if (!showRay)
        {
            return;
        }

        Gizmos.color = rayColor;
        coastPoint.Clear();

        foreach (Transform pointobj in coastHolder.GetComponentsInChildren<Transform>())
        {
            if (pointobj != this.transform)
            {
                coastPoint.Add(pointobj);
                Gizmos.DrawWireSphere(pointobj.position, wireSphereRadius);
            }

        }
    }


}
