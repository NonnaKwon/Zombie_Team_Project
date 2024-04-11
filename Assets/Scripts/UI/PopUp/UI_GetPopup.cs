using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GetPopup : PopUpUI
{
    UI_GetItemToken _itemPrefab;

    enum GameObjects
    {
        Items,
    }

    protected override void Awake()
    {
        base.Awake();
        _itemPrefab = Manager.Resource.Load<UI_GetItemToken>("Prefabs/UI/SubItem/UI_GetItemToken");
    }

    public void UpdateItemList(ItemData items, int count)
    {
        Debug.Log(_itemPrefab);
        Transform parent = GetUI(GameObjects.Items.ToString()).transform;
        UI_GetItemToken item = Instantiate(_itemPrefab, parent);
        item.SetData(items, count);
    }
}
