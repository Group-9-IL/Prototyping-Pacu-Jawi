using System.Collections;
using System.Collections.Generic;
// using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    private GachaItem currentItem;
    private PlayerMovement player;
    private Animator dropItem;
    private Animator fade;
    private float delayItem = 0f;
    public Image playerItem;
    private AudioManager audioManager;

    private void Start()
    {
        GameObject ItemUI = GameObject.Find("ItemUI");
        fade = ItemUI.GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
        player = GetComponent<PlayerMovement>();
        GameObject item = GameObject.Find("Items");
        dropItem = item.GetComponent<Animator>();
        GameObject playerItemObject = GameObject.Find("PlayerItemUI");
        if (playerItem == null)
        {
            playerItem = playerItemObject.GetComponent<Image>();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && ( currentItem != null))
        {
            UseItem();
            fade.SetBool("getItem",false);
        }

        if(delayItem > 0)
        {
            delayItem -= Time.deltaTime;
        }
    }

    private void UseItem()
    {

        if(currentItem.itemName == "Boost")
        {
            player.ItemBoost();
            audioManager.PlaySFX(SfxCondition.speedBoost);
        }else if(currentItem.itemName == "Clean Run")
        {
            player.ItemCleanRun();
            audioManager.PlaySFX(SfxCondition.cleanRun);
        }else if(currentItem.itemName == "Ram")
        {
            audioManager.PlaySFX(SfxCondition.ram);
            player.ItemRam();
        }

        currentItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gachaBox") && (currentItem == null) && delayItem <= 0)
        {  
            Debug.Log("Item didapatkan");
            delayItem = 5f;
            GachaBox gachaBoxInstance = other.gameObject.GetComponent<GachaBox>();
            currentItem = gachaBoxInstance.OpenBox();
            playerItem.sprite=currentItem.itemIcon;
            fade.SetBool("getItem",true);
            dropItem.SetBool("hasItemAnim", true);
        }
    }
}
