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
    public int holdingFrame;
    public PlayerInputEvent(string buttonName, int playerId,int holdingFrame)
    {
        this.buttonName = buttonName;
        this.playerId = playerId;
        this.holdingFrame = holdingFrame;
    }
}

public struct PlayerInputDownEvent
{
    public string buttonName;
    public int playerId;
    public PlayerInputDownEvent(string buttonName , int playerId)
    {
        this.buttonName = buttonName;
        this.playerId = playerId;
    }
}

public struct PlayerInputHoldEvent
{
    public string buttonName;
    public int playerId;
    public PlayerInputHoldEvent(string buttonName, int playerId)
    {
        this.buttonName = buttonName;
        this.playerId = playerId;
    }
}

public struct PlayerInputUpEvent
{
    public string buttonName;
    public int playerId;
    public PlayerInputUpEvent(string buttonName, int playerId)
    {
        this.buttonName = buttonName;
        this.playerId = playerId;
    }
}