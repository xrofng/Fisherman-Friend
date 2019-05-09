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

public struct PlayerInputEvent
{
    public string buttonName;
    public int playerId;
    public PlayerInputEvent(string buttonName , int playerId)
    {
        this.buttonName = buttonName;
        this.playerId = playerId;
    }
}