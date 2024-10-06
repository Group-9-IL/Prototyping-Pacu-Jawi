using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems;

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
}
