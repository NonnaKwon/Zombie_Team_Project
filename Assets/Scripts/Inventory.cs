using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int BulletCount
    {
        get { return _bullet; }
        set
        {
            _bullet = value;
            BulletChange?.Invoke();
        }
    }
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

    public event Action BulletChange;
    private List<ItemEntity> items = new List<ItemEntity>();
    public int ItemSize { get { return items.Count; } }
    private int _bullet = 0;

    private void Start()
    {

    }

    public void AddItem(ItemData item)
    {
        AddItem(item, 1);
    }

    public void AddItem(ItemData item,int count)
    {
        if (item.ItemName.Equals("bullet"))
        {
            BulletCount += count;
        }

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
                if (item.ItemName.Equals("bullet"))
                {
                    BulletCount--;
                }
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
