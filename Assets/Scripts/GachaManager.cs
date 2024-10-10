using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems;
    public GameObject gachaBox;

    private void Start()
    {
        //Instantiate(gachaBox, new Vector3(3f, 1f, 5f), Quaternion.identity);
        //ni command untuk spawner nya nanti buat coba doang
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
}
