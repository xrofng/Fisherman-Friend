using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishPool
{
    public List<FishSpawning> FishSpawnings = new List<FishSpawning>();
    private Dictionary<int, FishSpawning> cachedFishSpawnings = new Dictionary<int, FishSpawning>();

    public int totalSpawnRate = 0;

    public void Initialization()
    {
        foreach(FishSpawning fishSpawning in FishSpawnings)
        {
            if (!cachedFishSpawnings.ContainsKey(fishSpawning.Fish.fishId))
            {
                cachedFishSpawnings.Add(fishSpawning.Fish.fishId, fishSpawning);
            }
        }
        CalculateTotalSpawnRate();
    }

    public void CalculateTotalSpawnRate()
    {
        totalSpawnRate = 0;
        for (int i = 0; i < FishSpawnings.Count; i++)
        {
            totalSpawnRate += FishSpawnings[i].spawnRate;
        }
    }

    public int GetRandomFishIndex()
    {
        float ran = Random.Range(0, totalSpawnRate);
        float spawnPercentOffset = 0;
        for (int i = 0; i < FishSpawnings.Count; i++)
        {
            if (ran < FishSpawnings[i].spawnRate + spawnPercentOffset)
            {
                return i;
            }
            spawnPercentOffset += FishSpawnings[i].spawnRate;
        }
        Debug.LogWarning("error random fish");
        return 999;
    }

    public void RemoveFish(Fish remove)
    {
        FishSpawnings.Remove(cachedFishSpawnings[remove.fishId]);
        CalculateTotalSpawnRate();
    }
}

[System.Serializable]
public class FishSpawning
{
    public string name;
    public Fish Fish;
    public int spawnRate;
}