using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems;
    public List<Vector3> spawnPosition;
    public GameObject gachaBox;

    private int sumSpawned = 0;
    private TimerManager timerManager;

    private void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        SpawnGachaBox();
    }

    private void Update()
    {
        if(timerManager !=null && timerManager.getCurrentTime() > sumSpawned * 45)
        {
            SpawnGachaBox();
        }
    }

    public GachaItem RollGacha()
    {
        float totalWeight = 0f;

        foreach (var item in gachaItems)
        {
            totalWeight += item.dropRate;
        }

        float randomValue = UnityEngine.Random.Range(0f,totalWeight);
        float cumulativeWeight = 0f;
        
        foreach (var item in gachaItems)
        {
            cumulativeWeight += item.dropRate;
            if (randomValue < cumulativeWeight)
            {
                return item;
            }
        }
        return null;
    }

    private void SpawnGachaBox()
    {
        GachaBox[] existingGachaBoxes = FindObjectsOfType<GachaBox>();

        foreach (GachaBox box in existingGachaBoxes)
        {
            Destroy(box.gameObject);
        }
        int spawnCount = Mathf.Min(4,spawnPosition.Count);
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(gachaBox, spawnPosition[i], Quaternion.identity);
        }
        sumSpawned++;
    }
}
