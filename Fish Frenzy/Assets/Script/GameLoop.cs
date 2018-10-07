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
    public float Round_Time_Limit = 360;
    public float startCountDown = 4.5f;
    private float timeCountDown;
    public GameObject playerPrefab;
    private MaterialManager materialManager;

    public float Time_Minute
    {
        get { return timeCountDown / 60; }
    }
    public float Time_Second
    {
        get { return timeCountDown % 60; }
    }
    // Use this for initialization
    void Start () {

        materialManager = GetComponent<MaterialManager>();
        timeCountDown = Round_Time_Limit;

        
        StartCoroutine(CountDown(startCountDown));
    }
	
	// Update is called once per frame
	void Update () {
        if(state == GameState.beforeStart)
        {
            startCountDown -= Time.deltaTime;
        }
        else if (state == GameState.playing)
        {
            timeCountDown -= Time.deltaTime;
        }
        

    }

    void spawnPlayers()
    {
        List<int> takenPos = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            Player p = materialManager.InstantiatePlayer(playerPrefab, i).GetComponent<Player>();
            PortRoyal.Instance.player[i] = p;
            p.playerID = i + 1;
            p.gameObject.name = "Player" + p.playerID;
            int positionIndex = PortRoyal.Instance.randomSpawnPosIndex();

            while (takenPos.Contains(positionIndex))
            {
                positionIndex = PortRoyal.Instance.randomSpawnPosIndex();
            }

            p.gameObject.transform.position = PortRoyal.Instance.getSpwanPositionAtIndex(positionIndex);
            takenPos.Add(positionIndex);

            p.Initialization();
            
        }
    }

    IEnumerator CountDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spawnPlayers();
        state = GameState.playing;
        GUIManager.Instance.GrandText.enabled = false;
        MultiPlayerCamera._instance.Initialization();
    }
}
