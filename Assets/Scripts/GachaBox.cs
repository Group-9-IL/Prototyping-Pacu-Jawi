using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaBox : MonoBehaviour
{
    private GachaManager gachaManager;
    public Image PlayerItem;
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
            PlayerItem.sprite = obtainedItem.itemIcon;
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
