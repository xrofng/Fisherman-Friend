using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour {
    public Player targetPlayer;
    public bool followPlayer;
    public Transform levelCenter;
    private float _toPlayerSpeed;
    public float ToPlayerSpeed
    {
        get { return _toPlayerSpeed; }
        set { _toPlayerSpeed = value; }
    }
    private float _toCenterSpeed;
    public float ToCenterSpeed
    {
        get { return _toCenterSpeed; }
        set { _toCenterSpeed = value; }
    }
    public float ReachDistance;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
        followPlayer = !targetPlayer.Death;

        float speed = ToCenterSpeed;
        Vector3 targetPos = levelCenter.position;
       
        if (followPlayer)
        {
            speed = ToPlayerSpeed;
            targetPos = targetPlayer.gameObject.transform.position;
        }
        if ( Vector3.Distance(targetPos, transform.position) <=ReachDistance )
        {
            return;
        }
        Vector3 dir = Vector3.Normalize(targetPos - transform.position);
        transform.Translate(dir * speed * Time.deltaTime);

    }

    public void SetCamTarget(Player p, bool follow, Transform center)
    {
        targetPlayer = p;
        levelCenter = center;
        followPlayer = follow;
    }

    [Header("Debug")]
    public bool showRay = true;
    public Color rayColor = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawWireSphere(transform.position, ReachDistance);
    }
}
