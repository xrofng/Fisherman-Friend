using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Fisherman_Fuzzy : AgentFuzzy,MMEventListener<PlayerSpawnedEvent>
{
    protected override void Initialize()
    {
        base.Initialize();
        this.MMEventStartListening<PlayerSpawnedEvent>();
    }

    public void OnMMEvent(PlayerSpawnedEvent eventType)
    {
        foreach (Player inGamePlayer in FFGameManager.Instance.PortRoyal.Player)
        {
            // TODO if casted agent will use more than 2 times, cacdhe it to property
            if (inGamePlayer != agent.CastAgent<Agent_Fisherman>().OwnerPlayer)
            {
                Desirable_TargetPlayer_Fisherman desire_target = AddDesirable<Desirable_TargetPlayer_Fisherman>();
                desire_target.targetPlayer = inGamePlayer;
                desire_target.agent_Fisherman = agent.CastAgent<Agent_Fisherman>();
            }
        }
        InitFuzzy();
    }

    public GameObject GetAttackTarget()
    {
        List<GameObject> mostDesirabilityPlayers = new List<GameObject>();
        double maxDesire = double.MinValue;
        foreach (Desirable_TargetPlayer_Fisherman desire in GetDesirables<Desirable_TargetPlayer_Fisherman>())
        {
            if (desire.GetDesirability() > maxDesire)
            {
                maxDesire = desire.GetDesirability();
                mostDesirabilityPlayers.Clear();
            }
            if (desire.GetDesirability() == maxDesire)
            {
                mostDesirabilityPlayers.Add(desire.targetPlayer.gameObject);
            }
        }
        Debug.Log(agent.name + " find " + mostDesirabilityPlayers.Count + " max desirablity");
        if (mostDesirabilityPlayers.Count <= 1)
        {
            return mostDesirabilityPlayers[0];
        }

        return mostDesirabilityPlayers[(int)Random.Range(0, mostDesirabilityPlayers.Count)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
