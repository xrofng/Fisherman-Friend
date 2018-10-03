using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : PersistentSingleton<GameLoop>
{
    public float Round_Time_Limit = 360;
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

        spawnPlayers();
    }
	
	// Update is called once per frame
	void Update () {
        timeCountDown -= Time.deltaTime;
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
}
