using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Inventory : PopUpUI           
{
    private Inventory _inventory;
    List<UI_InvenItemToken> _invenItemInfo;
    enum GameObjects
    {
        Items,
        Cash
    }

    protected override void Awake()
    {
        base.Awake();
        _invenItemInfo = GetComponentsInChildren<UI_InvenItemToken>().ToList();
        foreach(UI_InvenItemToken token in _invenItemInfo)
        {
            token.SetActiveToken(false);
        }
        _inventory = Manager.Game.Player.GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        GetUI<TMP_Text>(GameObjects.Cash.ToString()).text = Manager.Game.Player.Coin.ToString();
        for(int i=0;i<_inventory.ItemSize;i++)
        {
            if(!_invenItemInfo[i].GetActiveTokwn())
                _invenItemInfo[i].SetActiveToken(true);
            int itemNum;
            ItemData item = _inventory.GetData(i, out itemNum);
            _invenItemInfo[i].SetData(item,itemNum);
        }
    }

}
