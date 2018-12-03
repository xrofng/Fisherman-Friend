using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_JellyFish : StageInteraction {

    public Vector3 bounceForce = new Vector3(0,5,0);
    public int ShockingFrame = 10;
    public AudioClip sfx_Shock;
    public List<AudioClip> sfx_Bounces = new List<AudioClip>();
    protected int recentSfxIndex;

    protected List<Player> ignoreBounceList = new List<Player>();
    public int ignoreBounceFrame = 4;

    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.FreezeFramesOnHit = ShockingFrame;
    }

    // Update is called once per frame
    protected override void Update () {
		
	}

    public override void OnPlayerCollide(Player _player)
    {
        if (ignoreBounceList.Contains(_player))
        {
            return;
        }
        print(_player._cPlayerState.IsJumping);
        if (_player._cPlayerState.IsJumping)
        {
            StartCoroutine(IgnorePlayerFor(ignoreBounceFrame, _player));
            // play animation feedback
            Animator.ChangeAnimState(1, true, 0);
            PlaySFX(RandomAudioClip());
            _player._cPlayerMovement.StartJumping(bounceForce);
        }
        else 
        {
            PlaySFX(sfx_Shock);
            hitBox.CauseDamage();
        }
    }

    AudioClip RandomAudioClip()
    {
        int auidoIndex = (int)Random.Range(0, sfx_Bounces.Count);
        while(auidoIndex == recentSfxIndex)
        {
            auidoIndex = (int)Random.Range(0, sfx_Bounces.Count);
        }
        return sfx_Bounces[auidoIndex];
    }

    IEnumerator IgnorePlayerFor(int frameDuration , Player _player)
    {
        ignoreBounceList.Add(_player);

        int frameCout = 0;
        while (frameCout < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCout += 1;
        }
        if (ignoreBounceList.Contains(_player))
        {
            ignoreBounceList.Remove(_player);
        }
    }
}
    

