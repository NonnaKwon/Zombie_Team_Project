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

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item.ItemName.Equals(item.ItemName))
            {
                items.RemoveAt(i);
            }
        }

        Debug.Log("일치하는 아이템이 없음");
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
