using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzySpawner : MonoBehaviour {
    public bool Frenzying;
    public Vector2 spawnTime;
    public Vector2 fallSpeed;
    public Vector2 amountRange;

    public List<Transform> SpawnPoints = new List<Transform>();
    public List<int> spawnedPoint = new List<int>();
    private float timeCount = 0;
    public float timeToNextWave = 10;

    public float timeAnimationOverhead = 9.3f;
    public float timeFrenzy= 60.0f;

    public GameObject whalePrefab;
    protected GameObject whale;
    protected Animator _whaleAnim;
    public Animator WhaleAnim
    {
        get
        {
            if (!_whaleAnim && whale)
            {
                _whaleAnim = whale.GetComponentInChildren<Animator>();
            }
            return _whaleAnim;
        }
    }
    protected AudioSource _SFX;
    public AudioSource SFX
    {
        get
        {
            if (!_SFX) { _SFX = GetComponent<AudioSource>(); }
            return _SFX;
        }
    }
    public int whaleAnimFrame;

    [Header("Other Class Ref")]
    protected GameLoop gameLoop;
    protected PortRoyal portRoyal;
    // Use this for initialization
    void Start () {
        whale = Instantiate(whalePrefab);
        gameLoop = FFGameManager.Instance.GameLoop;
        portRoyal = FFGameManager.Instance.PortRoyal;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Frenzying)
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            if (timeCount <= timeAnimationOverhead)
            {
                PlayWhaleAnimation();
            }
            if (timeCount <= 0)
            {
                int amountFish = (int)Random.Range(amountRange.x, amountRange.y);
                spawnedPoint.Clear();
                for (int i = 0; i < amountFish; i++)
                {
                    int spawnPointIndex = Random.Range(0, SpawnPoints.Count - 1);
                    while (spawnedPoint.Contains(spawnPointIndex))
                    {
                        spawnPointIndex = Random.Range(0, SpawnPoints.Count - 1);
                    }
                    spawnedPoint.Add(spawnPointIndex);

                    Vector3 spawnPos = SpawnPoints[spawnPointIndex].position;
                    spawnPos = sClass.setVector3(spawnPos, sClass.vectorComponent.y, transform.position.y);

                    SpawnFish(spawnPos);

                }
                timeCount = timeToNextWave;

            }
        }
	}

    void SpawnFish(Vector3 spawnPos)
    {
        Fish spawnFish = Instantiate(portRoyal.randomFish(), spawnPos, Random.rotation) as Fish;
        spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.x, 0);
        spawnFish.gameObject.transform.localEulerAngles = sClass.setVector3(spawnFish.gameObject.transform.localEulerAngles, sClass.vectorComponent.z, 0);
        spawnFish.ChangeState(Fish.fState.fall);
        //spawnFish.GetCollider<BoxCollider>().isTrigger = true;

        //spawnFish.gameObject.layer = LayerMask.NameToLayer("Fish");
        float f = Random.Range(fallSpeed.x, fallSpeed.y);
        spawnFish.FishJump(1, 10, Vector3.down, -f);
        //spawnFish._rigidbody.freezeRotation = true;
    }

    public void StartFrenzy(bool b)
    {
        Frenzying = b;
    }

    public void PlayWhaleAnimation()
    {
        if (!SFX.isPlaying)
        {
            SFX.Play();
        }
        StartCoroutine(WhaleAnimPlay(whaleAnimFrame));
    }

    IEnumerator WhaleAnimPlay(int frameDuration)
    {
        int frameCount = 0;
        WhaleAnim.SetBool("Jump",true);
        while (frameCount < frameDuration)
        {
            yield return new WaitForEndOfFrame();
            frameCount++;
        }
        WhaleAnim.SetBool("Jump",false);
    }

    // Draw Path
    [Header("Debug")]
    public Color rayColor;
    public float wireSphereRadius = 1.0f;
    public bool showRay = true;
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        SpawnPoints.Clear();

        foreach (Transform pointobj in transform.GetComponentsInChildren<Transform>())
        {
            if (pointobj != this.transform)
            {
                SpawnPoints.Add(pointobj);
                Gizmos.DrawWireSphere(pointobj.position, wireSphereRadius);
            }
           
        }
    }
}
