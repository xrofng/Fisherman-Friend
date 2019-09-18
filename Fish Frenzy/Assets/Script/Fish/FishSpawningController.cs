using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishPool
{
    public List<FishSpawning> FishSpawnings = new List<FishSpawning>();
    private Dictionary<Fish,FishSpawning> cachedFishSpawnings = new Dictionary<Fish, FishSpawning>();

    public int totalSpawnRate = 0;

    public void Initialization()
    {
        foreach(FishSpawning fishSpawning in FishSpawnings)
        {
            if (!cachedFishSpawnings.ContainsKey(fishSpawning.Fish))
            {
                Debug.Log("cache" + fishSpawning.Fish.name);
                cachedFishSpawnings.Add(fishSpawning.Fish, fishSpawning);
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
        Debug.Log(remove.fishId);
        Debug.Log(cachedFishSpawnings.ContainsKey(remove));
        FishSpawnings.Remove(cachedFishSpawnings[remove]);
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