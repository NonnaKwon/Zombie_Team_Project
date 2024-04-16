using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : InteracterController
{
    [Serializable]
    public class ItemEntity
    {
        public ItemData item;
        public int count;
    }

    [SerializeField] List<ItemEntity> items;
    UI_Shop _prefab;

    protected override void Start()
    {
        base.Start();
        _prefab = Resources.Load<UI_Shop>("Prefabs/UI/Popup/UI_Shop");
    }

    public override void Interact()
    {
        UI_Shop makePopup = Manager.UI.ShowPopUpUI(_prefab);
        makePopup.SetShopConnect(this);
        foreach(ItemEntity itemToken in items)
            makePopup.UpdateItemList(itemToken.item, itemToken.count);
    }

    public void UpdateData(ItemData data, int count)
    { 
        for(int i=0;i<items.Count;i++)
        {
            if (items[i].item.name.Equals(data.name))
                items[i].count = count;
        }
    }
}
