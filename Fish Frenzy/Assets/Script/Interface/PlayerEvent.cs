using UnityEngine;
using System.Collections;



public struct PlayerSpawnedEvent
{
    Player[] players;
    public PlayerSpawnedEvent(Player[] spawnedPlayer)
    {
        players = spawnedPlayer;
    }
}