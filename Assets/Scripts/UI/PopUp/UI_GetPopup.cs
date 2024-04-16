using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDropTable;

public class UI_GetPopup : PopUpUI
{
    UI_GetItemToken _itemPrefab;
    Dictionary<ItemData, int> _getItems;

    enum GameObjects
    {
        Items,
    }

    protected override void Awake()
    {
        base.Awake();
        _getItems = new Dictionary<ItemData, int>();
        _itemPrefab = Manager.Resource.Load<UI_GetItemToken>("Prefabs/UI/SubItem/UI_GetItemToken");
    }

    public void UpdateItemList(ItemData items, int count)
    {
        if (!_getItems.ContainsKey(items))
            _getItems[items] = 0;
        _getItems[items] += count;
    }

    public void CreateList()
    {
        foreach (KeyValuePair<ItemData, int> token in _getItems)
        {
            Transform parent = GetUI(GameObjects.Items.ToString()).transform;
            UI_GetItemToken item = Instantiate(_itemPrefab, parent);
            item.SetData(token.Key, token.Value);
        }
    }
}
