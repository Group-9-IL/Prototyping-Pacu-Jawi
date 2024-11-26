using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{

    private GachaItem currentItem;
    private PlayerMovement player;
    private Animator dropItem;
    private float delayItem = 0f;
    private bool hasItem = false;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && ( currentItem != null))
        {
            UseItem();
            Debug.Log("Make Item");
        }

        if(delayItem > 0)
        {
            delayItem -= Time.deltaTime;
        }
    }

    private void UseItem()
    {

        if(currentItem.itemName == "Mud")
        {
            player.ItemMud();
        }else if(currentItem.itemName == "Boost")
        {
            player.ItemBoost();
        }else if(currentItem.itemName == "Clean Run")
        {
            player.ItemCleanRun();
        }else if(currentItem.itemName == "Ram")
        {
            player.ItemRam();
        }

        currentItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gachaBox") && (currentItem == null) && delayItem <= 0)
        {
            delayItem = 5f;
            GachaBox gachaBoxInstance = other.gameObject.GetComponent<GachaBox>();
            currentItem = gachaBoxInstance.OpenBox();
            dropItem.SetBool("hasItemAnim",true);
            hasItem=true;
            Debug.Log(currentItem.itemName);

        }
    }
}
