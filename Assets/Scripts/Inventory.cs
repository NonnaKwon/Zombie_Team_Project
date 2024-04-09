using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private class ItemEntity
    {
        public ItemData Item;
        public int Count;

        public ItemEntity(ItemData item,int count)
        {
            Item = item;
            Count = count;
        }
    }

    private List<ItemEntity> items = new List<ItemEntity>();
    public int ItemSize { get { return items.Count; } }

    private void Start()
    {

    }

    public void AddItem(ItemData item)
    {
        for(int i=0;i<items.Count;i++)
        {
            if (items[i].Item.ItemName.Equals(item.ItemName))
            {
                items[i].Count++;
                return;
            }
        }

        ItemEntity itemToken = new ItemEntity(item,1);
        items.Add(itemToken);
    }

    public void AddItem(ItemData item,int count)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item.ItemName.Equals(item.ItemName))
            {
                items[i].Count += count;
                return;
            }
        }

        ItemEntity itemToken = new ItemEntity(item, count);
        items.Add(itemToken);
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item.ItemName.Equals(item.ItemName))
            {
                if (items[i].Count == 1)
                    items.RemoveAt(i);
                else
                    items[i].Count--;
            }
        }
    }

    public ItemData GetData(int index, out int count)
    {
        if (items.Count == index)
        {
            count = 0;
            return null;
        }
        count = items[index].Count;
        return items[index].Item;
    }
}
