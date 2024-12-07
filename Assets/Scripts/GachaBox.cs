using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaBox : MonoBehaviour
{
    private GachaManager gachaManager;
    // private PlayerItem playerItemObject;
    public Image playerItem;
    private void Start()
    {
        gachaManager = FindObjectOfType<GachaManager>();
        // playerItemObject = FindObjectOfType<PlayerItem>();
    }

    public GachaItem OpenBox()
    {
        Destroy(gameObject);
        GachaItem obtainedItem = gachaManager.RollGacha();
        return obtainedItem;

    }
}
