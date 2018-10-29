using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : PlayerAbility {

    private float holdToThrow;
    public float forwardMultiplier = 50.0f;
    public float upMultiplier = 5.0f;

    private float aimRadius;
    public bool aiming;

    
    public Transform aimArrow;
    protected Transform _aimArrow;
    public Vector3 arrowPositioningOffset = new Vector3(0, -0.002f, -0.0279f);
    /// <summary>
    /// The number of rays cast vertically to check for floor. Must be 2 or higher.
    /// </summary> 
    public int NumberOfVerticalRays = 100;
    public float rayDistanceFrequent = 0.1f;
    private List<float> lowDetected = new List<float>();
    public float additionOverUp = 0.25f;

    // Use this for initialization
    protected override void Start () {
        
        Initialization();
        
    }

    protected override void Initialization()
    {
        base.Initialization();
        _aimArrow = Instantiate(aimArrow);
        _aimArrow.transform.SetParent( _player.getPart(Player.ePart.body) );
        _aimArrow.GetComponent<SpriteRenderer>().color = PortRoyal.Instance.playerColor[_player.playerID-1];
        _aimArrow.localPosition =  arrowPositioningOffset;
        _aimArrow.gameObject.SetActive(false);
    }

        // Update is called once per frame
    void Update () {
        if(_player.state == Player.eState.ground)
        {
            if (_player.IgnoreInputForAbilities || IgnoreInput)
            {
                return;
            }
            ThrowFish();
        }
    }

    void ThrowFish()
    {
        string thro = "Throw" + _player.playerID;
        if (_player.mainFish == null)
        {
            return;
        }
        if (Input.GetButtonDown(thro))
        {
            holdToThrow = 0;
            GetCrossZComponent<PlayerFishInteraction>().SetMainFishTransformAsPart(Player.ePart.rightArm, Player.ePart.body , true);
            _player.mainFish.SnapAimingTransform();
            _player.FreezingMovement = true;
            EvaluateAimArrowPosition();
            _aimArrow.gameObject.SetActive(true);
            aiming = true;
        }
        else if (Input.GetButton(thro))
        {
            holdToThrow += Time.deltaTime;
            EvaluateAimArrowPosition();
            AimAssist();
        }
        else if (Input.GetButtonUp(thro))
        {
            _player.mainFish.lastHoldPoition = _player.mainFish.transform.position;
            GetCrossZComponent<PlayerFishInteraction>().SetMainFishTransformAsPart(Player.ePart.body, Player.ePart.body , true);

            GetCrossZComponent<PlayerFishInteraction>().SetFishCollidePlayer(_player.mainFish, _player, false);

            _player.mainFish.FishThrow(holdToThrow ,forwardMultiplier,upMultiplier );
            _player.mainFish.changeState(Fish.fState.threw);
            GetCrossZComponent<PlayerFishInteraction>().SetHoldFish(false);
            
        }
    }

    void EvaluateAimArrowPosition()
    {
        List<Collider2D> hitColliders = new List<Collider2D>();
        Vector3 rayPos = transform.position+ Vector3.up;
        lowDetected.Clear();
        
        for (int i = 0; i < NumberOfVerticalRays; i++)
        {
            rayPos += _player.getPart(Player.ePart.body).TransformDirection(-Vector3.forward) * rayDistanceFrequent ;
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            Color lineColor = Color.magenta;
            if (Physics.Raycast(rayPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.tag == "Ground")
                {
                    lowDetected.Add(hit.point.y);
                }
            }
        }

        // Get highest Y
        float highest = float.MinValue;
        foreach(float low in lowDetected)
        {
            if(low > highest)
            {
                highest = low;
            }
        }
        // set position Y
        _aimArrow.transform.position = sClass.setVector3(_aimArrow.transform.position, sClass.vectorComponent.y, highest + additionOverUp);
    }



    public void ChangeToUnAim()
    {
        _player.FreezingMovement = false;
        _aimArrow.gameObject.SetActive(false);
        aiming = false;
    }

    void AimAssist()
    {
        print(GetCrossZComponent<PlayerMovement>().GetTurningDegree());
    }

    
    //void AimAssist()
    //{
    //    //aim assist
    //    for (int i = 0; i < 4; i++)
    //    {
    //        float[] angle = { 1000, 1000, 1000, 1000 };
    //        Vector3[] direction = new Vector3[4];
    //        aiming = false;
    //        if (i + 1 != _player.playerID)
    //        {
    //            Player target = PortRoyal.Instance.player[i];
    //            direction[i] = target.transform.position - this.transform.position;
    //            angle[i] = Vector3.Angle(direction[i], _player.playerForward);
    //            bool found = false;
    //            if (angle[i] < aimRadius * 0.5f)
    //            {
    //                found = true;
    //                print(target.gameObject.name);
    //            }
    //            if (found)
    //            {
    //                aiming = true;
    //                int index = sClass.findMinOfArray(angle);
    //                _player.getPart(Player.ePart.body).transform.rotation = Quaternion.LookRotation(direction[index], Vector3.up);
    //                print(found);
    //            }
    //        }
    //    }
    //}
}
