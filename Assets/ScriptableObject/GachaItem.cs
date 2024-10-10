using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="gachaItem", menuName ="gachaItem")]
public class GachaItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public float dropRate;
}
