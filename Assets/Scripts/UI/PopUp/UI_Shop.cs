using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class UI_Shop : PopUpUI
{
    UI_ItemToken _itemPrefab;
    ShopController _connectShop;
    
    enum GameObjects
    {
        Items,
    }

    protected override void Awake()
    {
        base.Awake();
        _itemPrefab = Manager.Resource.Load<UI_ItemToken>("Prefabs/UI/SubItem/UI_ItemToken");
    }

    public void SetShopConnect(ShopController shop)
    {
        _connectShop = shop;
    }

    public void UpdateItemList(ItemData items,int count)
    {
        Transform parent = GetUI(GameObjects.Items.ToString()).transform;
        UI_ItemToken item = Instantiate(_itemPrefab, parent);
        item.SetItem(items,count,_connectShop);
    }

    public void ClickClose()
    {
        Close();
    }

}
