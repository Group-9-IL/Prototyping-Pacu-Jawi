using System.Collections;
using System.Collections.Generic;
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
    private ScreenEffects screenEffects;

    private void Start()
    {
        GameObject ItemUI = GameObject.Find("ItemUI");
        fade = ItemUI.GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
        player = GetComponent<PlayerMovement>();
        GameObject item = GameObject.Find("Items");
        dropItem = item.GetComponent<Animator>();
        GameObject playerItemObject = GameObject.Find("PlayerItemUI");
        screenEffects = FindObjectOfType<ScreenEffects>();
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
            screenEffects.ActivateEffect(ScreenEffects.EffectType.SpeedBoost, 3f);
        }else if(currentItem.itemName == "Clean Run")
        {
            player.ItemCleanRun();
            audioManager.PlaySFX(SfxCondition.cleanRun);
            screenEffects.ActivateEffect(ScreenEffects.EffectType.CleanRun, 3f);
        }else if(currentItem.itemName == "Ram")
        {
            audioManager.PlaySFX(SfxCondition.ram);
            player.ItemRam();
            screenEffects.ActivateEffect(ScreenEffects.EffectType.Ram, 1f);
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
            audioManager.PlaySFX(SfxCondition.hit);
            playerItem.sprite=currentItem.itemIcon;
            fade.SetBool("getItem",true);
            dropItem.SetBool("hasItemAnim", true);
        }
    }
}
