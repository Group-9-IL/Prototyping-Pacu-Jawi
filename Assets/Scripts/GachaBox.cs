using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBox : MonoBehaviour
{
    private GachaManager gachaManager;

    private void Start()
    {
        gachaManager = FindObjectOfType<GachaManager>();
    }

    public void OpenBox()
    {

        GachaItem obtainedItem = gachaManager.RollGacha();

        if (obtainedItem != null)
        {
            Debug.Log("You Got :"+ obtainedItem.itemName);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
