using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
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
    public float timeCountDown;

    public float timeBeforeChangeScene = 2.5f;
    public bool timeUp;
    public bool sceneChanging;
    public GameObject playerPrefab;
    public GameObject playerBotPrefab;
    public CamTarget playerFollowPrefab;
    public Transform LevelCenter;
    protected FrenzySpawner _frenzySpawner;
    public FrenzySpawner FrenzySpawner
    {
        get
        {
            if(_frenzySpawner == null)
            {
                _frenzySpawner = FindObjectOfType<FrenzySpawner>();
            }
            return _frenzySpawner;
        }
    }

    [Header("Other Class Ref")]
    protected MultiPlayerCamera multiplayerCamera;
    protected FocusCamera focusCamera;
    protected PortRoyal portRoyal;
    protected GUIManager guiManager;

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
        multiplayerCamera = FFGameManager.Instance.MultiplayerCamera;
        portRoyal = FFGameManager.Instance.PortRoyal;
        guiManager = FFGameManager.Instance.GUIManager;
        focusCamera = FFGameManager.Instance.FocusCamera;

        timeCountDown = Round_Time_Limit;

        StartCoroutine(CountDown(startCountDown));
        StartCoroutine(ieSpawnPlayer(startCountDown));
    }

    public void Reset()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        Update_Game();
    }

    void Update_Game()
    {
        if (state == GameState.beforeStart)
        {
            startCountDown -= Time.deltaTime;
            if (startCountDown < 1)
            {
                multiplayerCamera.MultiCamEnable = true;
            }
        }
        else if (state == GameState.playing || state == GameState.gameEnd)
        {
            timeCountDown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            timeCountDown = 46;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            timeCountDown = 1;
        }

        CheckTimeUp();
    }

    void OnChangeScene()
    {
        MatchResult.Instance.KnockerObjToName();
        multiplayerCamera.enabled = false;
        portRoyal.enabled = false;
        guiManager.enabled = false;
    }

    void CheckTimeUp()
    {
        if (timeCountDown < -timeBeforeChangeScene && !sceneChanging)
        {
            sceneChanging = true;            
            FrenzySpawner.StartFrenzy(false);
            Initiate.Fade("Result", Color.white, 5.0f);
            OnChangeScene();
            
        }
      
        if (timeCountDown <= 0)
        {
            state = GameState.gameEnd;
            guiManager.GrandText.enabled = true;
            timeUp = true;
        }
    }

    List<int> takenPos = new List<int>();
    void SpawnPlayers(int playerID, GameObject spawnCharacter)
    {
        PlayerData playerData = PlayerData.Instance;
        Player p = MaterialManager.Instance.InstantiatePlayer(spawnCharacter, 
            playerData.playerSkinId[playerID], playerData.hatId[playerID]).GetComponent<Player>();
        portRoyal.Player[playerID] = p;
        p.playerID = playerID + 1;
        p.gameObject.name = "Player" + p.playerID;
        int positionIndex = portRoyal.RandomSpawnPosIndex();

        while (takenPos.Contains(positionIndex))
        {
            positionIndex = portRoyal.RandomSpawnPosIndex();
        }

        p.gameObject.transform.position = portRoyal.getSpawnPositionAtIndex(positionIndex);
        takenPos.Add(positionIndex);

        p.Initialization();

        CamTarget c = Instantiate(playerFollowPrefab,LevelCenter.position,Quaternion.identity) as CamTarget;
        c.SetCamTarget(p, true,LevelCenter);
        c.ToPlayerSpeed = multiplayerCamera.speedToPlayer;
        c.ToCenterSpeed = multiplayerCamera.speedToCenter;
        multiplayerCamera.AddTarget(c.transform);
        focusCamera.MoveCameraTo(p.gameObject.transform.position,true);
    }

    IEnumerator ieSpawnPlayer(float waitTime)
    {
        PlayerData playerData = PlayerData.Instance;
        multiplayerCamera.enabled = true;
        multiplayerCamera.ClearTarget();
        float _playerSpawnRate = playerSpawnRate * playerData.maxNumPlayer / (playerData.numPlayer + playerData.numBot);
        yield return new WaitForSeconds(_playerSpawnRate);
        for (int i = 0; i < PlayerData.Instance.numPlayer; i++)
        {
            SpawnPlayers(i, playerPrefab);
            MMEventManager.TriggerEvent(new PlayerSpawnedEvent(i, portRoyal.Player));
            yield return new WaitForSeconds(_playerSpawnRate);
        }
        for (int i = PlayerData.Instance.numPlayer; i < PlayerData.Instance.numBot+ PlayerData.Instance.numPlayer; i++)
        {
            SpawnPlayers(i, playerBotPrefab);
            MMEventManager.TriggerEvent(new PlayerSpawnedEvent(i, portRoyal.Player));
            yield return new WaitForSeconds(_playerSpawnRate);
        }
        focusCamera.MoveCameraTo(multiplayerCamera.GetNewPosition(),false);
        MMEventManager.TriggerEvent(new PlayerSpawnedEvent(4,portRoyal.Player));
    }

    IEnumerator CountDown(float waitTime)
    { 
        yield return new WaitForSeconds(waitTime );
        state = GameState.playing;
        guiManager.GrandText.enabled = false;
    }

    
}
