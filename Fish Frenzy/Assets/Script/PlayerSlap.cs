using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlap : PlayerAbility {

    public MeleeHitBox hitBox;
    protected bool attacking;
    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
            hitBox.gameObject.SetActive(value);
        }
    }

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.gameObject.layer = LayerMask.NameToLayer("Fish" + _player.playerID);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.state == Player.eState.ground)
        {
            SlapFish();
        }
    }

    // Update is called once per frame
    void SlapFish() {
        string slap = "Slap" + _player.playerID;
        if (_player.mainFish == null)
        {
            return;
        }
        if (Input.GetButtonDown(slap))
        {
            //hitBox.center = _player.mainFish.hitboxCenter;
            //hitBox.size = _player.mainFish.hitboxSize;
            hitBox.DamageCaused = _player.mainFish.attack;
            if (!Attacking)
            {
                StartCoroutine(HitBoxEnable(2));
            }
        }
        else if (Input.GetButton(slap))
        {
           
        }
        else if (Input.GetButtonUp(slap))
        {
            
        }
    }

    IEnumerator HitBoxEnable(float time)
    {
        Attacking = true;
        yield return new WaitForSeconds(time);
        Attacking = false;

    }
}
