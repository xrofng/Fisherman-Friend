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

    [Header("Drowning")]
    public float drowningSpeed = 4;
    public float sharkDetectRadius = 4;
    public float reachDistance = 0.5f;
    protected bool sharkDetected = false;
    public int drowningDirection = 1;
    public Color rayColor;
    private Vector3 originPosition;
    public Transform sharkTransform;


    // Use this for initialization
    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
        hitBox.FreezeFramesOnHit = ShockingFrame;
        originPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update () {

        SharkDetection();
    }

    public override void OnPlayerCollide(Player _player)
    {
        if (ignoreBounceList.Contains(_player))
        {
            Debug.Log("No more bounce");
            return;
        }

        if (_player._cPlayerState.IsJumping)
        {
            StartCoroutine(IgnorePlayerFor(ignoreBounceFrame, _player));
            // play animation feedback
            Animation.ChangeAnimState(1, true, 0);
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

    void SharkDetection()
    {
        if (Vector3.Distance(sharkTransform.position, transform.position) < sharkDetectRadius)
        {
            if (!sharkDetected)
            {
                Animation.ChangeAnimState(2);
                sharkDetected = true;
                Collider.enabled = !sharkDetected;
            }
        }
        else
        {
            if (sharkDetected)
            {
                Animation.ChangeAnimState(4);
                sharkDetected = false;
                Collider.enabled = !sharkDetected;
            }
        }
    }

    // Draw Shark DetectRadius
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawWireSphere(this.transform.position, sharkDetectRadius);
    }
}
    

