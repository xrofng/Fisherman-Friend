using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : PersistentSingleton<GameLoop>
{
    public enum GameState
    {
        beforeStart = 0,
        playing,
        gameEnd
    }
    public GameState state = GameState.beforeStart;
    
    public float Round_Time_Limit = 300;
    public float startCountDown = 4.5f;
    public float playerSpawnRate = 0.55f;
    private float timeCountDown;
    public float timeBeforeChangeScene = 2.5f;
    public bool timeUp;
    public bool sceneChanging;
    public GameObject playerPrefab;
    public CamTarget playerFollowPrefab;
    public Transform LevelCenter;
    public FrenzySpawner FrenzySpawner;
    private MaterialManager materialManager;

    public float Time_Minute
    {
        get { return timeCountDown / 60; }
    }
    public float Time_Second
    {
        get { return timeCountDown % 60; }
    }
    public float TimeInSecond
    {
        get { return timeCountDown; }
    }

    // Use this for initialization
    void Start () {
        materialManager = GetComponent<MaterialManager>();
        timeCountDown = Round_Time_Limit;

        StartCoroutine(CountDown(startCountDown));
        StartCoroutine(SpawnPlayer(startCountDown));

    }

    public void Reset()
    {
    }

    // Update is called once per frame
    void Update () {


        if(state == GameState.beforeStart)
        {
            startCountDown -= Time.deltaTime;
            if(startCountDown < 1)
            {
                MultiPlayerCamera.Instance.MultiCamEnable = true;
            }
        }
        else if (state == GameState.playing || state == GameState.gameEnd)
        {
            timeCountDown -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            timeCountDown = 62;
        }
        CheckTimeUp();
    }

    void OnChangeScene()
    {
        MatchResult.Instance.KnockerObjToName();
        MultiPlayerCamera.Instance.enabled = false;
        PortRoyal.Instance.enabled = false;
        GUIManager.Instance.enabled = false;
        GameLoop.Instance.enabled = false;
    }

    void CheckTimeUp()
    {
        if (timeCountDown < -timeBeforeChangeScene && !sceneChanging)
        {
            sceneChanging = true;            
            FrenzySpawner.StartFrenzy(false);
            Initiate.Fade("Result", Color.white, 2.0f);
            OnChangeScene();
            
        }
        if (timeCountDown < 60)
        {
            FrenzySpawner.StartFrenzy(true);

        }
        if (timeCountDown <= 0)
        {
            state = GameState.gameEnd;
            GUIManager.Instance.GrandText.enabled = true;
            timeUp = true;
        }
    }

    List<int> takenPos = new List<int>();
    void spawnPlayers(int playerID)
    {
        Player p = materialManager.InstantiatePlayer(playerPrefab, playerID).GetComponent<Player>();
        PortRoyal.Instance.Player[playerID] = p;
        p.playerID = playerID + 1;
        p.gameObject.name = "Player" + p.playerID;
        int positionIndex = PortRoyal.Instance.randomSpawnPosIndex();

        while (takenPos.Contains(positionIndex))
        {
            positionIndex = PortRoyal.Instance.randomSpawnPosIndex();
        }

        p.gameObject.transform.position = PortRoyal.Instance.getSpwanPositionAtIndex(positionIndex);
        takenPos.Add(positionIndex);

        p.Initialization();

        CamTarget c = Instantiate(playerFollowPrefab,LevelCenter.position,Quaternion.identity) as CamTarget;
        c.SetCamTarget(p, true,LevelCenter);
        c.ToPlayerSpeed = MultiPlayerCamera.Instance.speedToPlayer;
        c.ToCenterSpeed = MultiPlayerCamera.Instance.speedToCenter;
        MultiPlayerCamera.Instance.AddTarget(c.transform);
        FocusCamera.Instance.MoveCameraTo(p.gameObject.transform.position,true);
    }

    IEnumerator SpawnPlayer(float waitTime)
    {
        for (int i = 0; i < PortRoyal.Instance.numPlayer; i++)
        {
            yield return new WaitForSeconds(playerSpawnRate);
            spawnPlayers(i);
        }
        yield return new WaitForSeconds(playerSpawnRate);
        FocusCamera.Instance.MoveCameraTo(MultiPlayerCamera.Instance.GetNewPosition(),false);
    }
    IEnumerator CountDown(float waitTime)
    { 
        yield return new WaitForSeconds(waitTime );
        state = GameState.playing;
        GUIManager.Instance.GrandText.enabled = false;
    }

    
}
