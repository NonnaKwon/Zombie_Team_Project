using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public string itemName;

    [System.Serializable] // Á÷·ÄÈ­
    public struct Stat
    {
        public string name;
        public int value;
    }

    public List<Stat> stats = new List<Stat>();

    public int maxStack;
    public int price;

    public Sprite icon;
    public Transform prefab;

    [Multiline]
    public string description;
}
