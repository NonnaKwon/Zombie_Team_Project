using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Inventory : PopUpUI           
{
    private Inventory _inventory;
    List<UI_InvenItemToken> _invenItemInfo;
    private int _count = 0;
    private bool isInit = false;
    enum GameObjects
    {
        Items,
        Cash
    }

    protected override void Awake()
    {
        base.Awake();
        _inventory = Manager.Game.Player.GetComponent<Inventory>();
    }


    public void SetInit()
    {
        if (isInit)
            return;
        _invenItemInfo = GetComponentsInChildren<UI_InvenItemToken>().ToList();
        foreach (UI_InvenItemToken token in _invenItemInfo)
        {
            token.SetActiveToken(false);
        }
        isInit = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(!isInit)
        {
            return;
        }
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        GetUI<TMP_Text>(GameObjects.Cash.ToString()).text = Manager.Game.Player.Coin.ToString();
        for (int i=0;i<_inventory.ItemSize;i++)
        {
            if(!_invenItemInfo[i].GetActiveToken())
                _invenItemInfo[i].SetActiveToken(true);
            ItemData item = _inventory.GetData(i, out int itemNum);
            _invenItemInfo[i].SetData(item,itemNum);
        }
        while (_count-- > _inventory.ItemSize)
            _invenItemInfo[_count].SetActiveToken(false);
        _count = _inventory.ItemSize;
    }

}
