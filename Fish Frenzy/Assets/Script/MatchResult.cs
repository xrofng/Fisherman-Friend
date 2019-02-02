using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchResult : PersistentSingleton<MatchResult>
{
    [Header("Kill Count")]
    public bool unTagEnable = false;
    public float untagAttackerDuration = 5.0f;
    public int maxNumPlayer = 4;
    public int numPlayer = 4;

    protected GUIManager _guiManager;
    public GUIManager GUIManager
    {
        get
        {
            if (_guiManager == null)
            {
                _guiManager = FFGameManager.Instance.GUIManager;
            }
            return _guiManager;
        }
    }

    private List<List<string>> knockByList_Name = new List<List<string>>();
    public List<List<string>> KnockByList_Name
    {
        get
        {
            if (knockByList_Name.Count == 0)
            {
                for (int i = 0; i < numPlayer; i++)
                {
                    knockByList_Name.Add(new List<string>());
                }
            }
            return knockByList_Name;
        }
    }

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
        ScoreChangeDisplay(recieveAttackID, knocker);
        PrintKnockBy(recieveAttackID);
    }

    public void ScoreChangeDisplay(int recieveAttackID,GameObject knocker)
    {
        if (!knocker.gameObject.GetComponent<Player>())
        {
            return;
        }
        if(knocker.gameObject.GetComponent<Player>().playerID != recieveAttackID)
        {
            GUIManager.AddScoreChange(knocker.gameObject.GetComponent<Player>().playerID - 1, 1);
        }
        GUIManager.AddScoreChange(recieveAttackID - 1, -1);
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
        if (lastAttack.Contains(attacker) && unTagEnable)
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

    public void KnockerObjToName()
    {
        for (int i = 0; i < numPlayer; i++)
        {
            foreach (GameObject go in KnockByList[i])
            {
                KnockByList_Name[i].Add(go.name);
            }
            PrintKnockBy_Name(i);
        }
        
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
    public void PrintKnockBy_Name(int playerID)
    {
        if (!printLog) { return; }
        string s = "P" + playerID + "Knock byN ";
        foreach (string st in KnockByList_Name[playerID ])
        {
            s += st + ">";
        }
        print(s);
    }
}
