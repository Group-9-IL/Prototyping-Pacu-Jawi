using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems;
    public List<Vector3> spawnPosition;
    public GameObject gachaBox;

    private int sumSpawned = 0;

    private void Start()
    {
        SpawnGachaBox();
    }

    private void Update()
    {
        if(TimerManager.Instance.getCurrentTime() > sumSpawned * 45)
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

        float randomValue = Random.Range(0f,totalWeight);
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

        for (int i = 0; i < 4; i++)
        {
            Instantiate(gachaBox, spawnPosition[i], Quaternion.identity);
        }
        sumSpawned++;
    }
}
