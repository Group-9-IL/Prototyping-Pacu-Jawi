using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBox : MonoBehaviour
{
    public GachaManager gachaManager;
    // Start is called before the first frame update
    public void openBox()
    {
        GachaItem obtainedItem = gachaManager.RollGacha();

        if (obtainedItem != null)
        {
            Debug.Log("You Got :"+ obtainedItem.itemName);
        }

    }
}
