using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public string ItemName;
    public int price;
    public string info;
}
