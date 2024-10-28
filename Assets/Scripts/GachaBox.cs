using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaBox : MonoBehaviour
{
    private GachaManager gachaManager;
    public Image playerItem;
    private void Start()
    {
        gachaManager = FindObjectOfType<GachaManager>();
        GameObject playerItemObject = GameObject.Find("PlayerItem");
        if (playerItem == null)
        {
            playerItem = playerItemObject.GetComponent<Image>();
        }else
        {
            Debug.LogError("PlayerItem tidak ditemukan");
        }
    }

    public void OpenBox()
    {

        GachaItem obtainedItem = gachaManager.RollGacha();

        if (obtainedItem != null)
        {
            Debug.Log("You Got :"+ obtainedItem.itemName);
            playerItem.sprite = obtainedItem.itemIcon;
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
