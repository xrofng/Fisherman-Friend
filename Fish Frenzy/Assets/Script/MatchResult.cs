using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchResult : PersistentSingleton<MatchResult>
{
    [Header("Kill Count")]
    public float untagAttackerDuration = 5.0f;
    public int maxNumPlayer = 4;
    public int numPlayer = 4;
    
    private List<List<GameObject>> knockByList = new List<List<GameObject>>();
    public List<List<GameObject>> KnockByList
    {
        get
        {
            if (knockByList.Count == 0)
            {
                for (int i = 0; i < numPlayer; i++)
                {
                    knockByList.Add(new List<GameObject>());
                }
            }
            return knockByList;
        }
    }
    private List<List<GameObject>> latestAttackerList = new List<List<GameObject>>();
    public List<List<GameObject>> LatestAttackerList
    {
        get
        {
            if (latestAttackerList.Count == 0)
            {
                for (int i = 0; i < numPlayer; i++)
                {
                    latestAttackerList.Add(new List<GameObject>());
                }
            }
            return latestAttackerList;
        }
    }

    [Header("Debug")]
    public bool printLog = false;

    public void StoreKnocker(int recieveAttackID, GameObject knocker)
    {
        if (knocker)
        {
            KnockByList[recieveAttackID - 1].Add(knocker);
        }
        PrintKnockBy(recieveAttackID);
    }

    public void StoreAttacker(int recieveAttackID, GameObject attacker)
    {
        StartCoroutine(ieStoreAttacker(recieveAttackID, attacker, untagAttackerDuration));
    }

    IEnumerator ieStoreAttacker(int recieveAttackID, GameObject attacker, float timeDuration)
    {
        List<GameObject> lastAttack = LatestAttackerList[recieveAttackID - 1];
        if (lastAttack.Contains(attacker))
        {
            lastAttack.Remove(attacker);
        }
        lastAttack.Add(attacker);
        PrintLatestAttack(recieveAttackID);
        yield return new WaitForSeconds(timeDuration);
        if (lastAttack.Contains(attacker))
        {
            lastAttack.Remove(attacker);
        }
        

    }
    public void ClearRecentDamager(int playerID)
    {
        LatestAttackerList[playerID - 1].Clear();
    }
   public GameObject GetLatestDamager(int playerID, bool onlyEnemyPlayer)
    {
        GameObject latest = null;
        foreach (GameObject go in LatestAttackerList[playerID - 1])
        {
            if (!onlyEnemyPlayer)
            {
                latest = go;
            }
            else if (go.GetComponent<Player>())
            {
                latest = go;
            }          
        }
        return latest;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

       

    }

    public void PrintLatestAttack(int playerID)
    {
        if (!printLog) { return; }
        string s = "P"+playerID+ "Hit by ";
        foreach (GameObject go in LatestAttackerList[playerID-1])
        {
            s += go.name + ">";
        }
        print(s);
    }

    public void PrintKnockBy(int playerID)
    {
        if (!printLog) { return; }
        string s = "P" + playerID + "Knock by ";
        foreach (GameObject go in KnockByList[playerID-1])
        {
            s += go.name + ">";
        }
        print(s);
    }
}
