using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_QuickSlot : BaseUI
{
    Inventory _inventory;
    List<UI_QuickToken> _quickItemInfo;
    int[] weaponSlotRange = { 0, 1 };
    int[] usableSlotRange = { 2, 7 };
    enum Gameobjects
    {

    }

    protected override void Awake()
    {
        base.Awake();
        _quickItemInfo = GetComponentsInChildren<UI_QuickToken>().ToList();
        foreach (UI_QuickToken token in _quickItemInfo)
        {
            token.SetActiveToken(false);
        }
        _inventory = Manager.Game.Player.GetComponent<Inventory>();
    }

    private void Start()
    {
    }

    public void AddQuickSlot(ItemData item,int count)
    {
        WeaponData weapon = item as WeaponData;
        if(weapon != null) //무기아이템
        {
            for(int i = weaponSlotRange[0] ; i<= weaponSlotRange[1] ; i++)
            {
                if(!_quickItemInfo[i].GetActiveToken())
                {
                    _quickItemInfo[i].SetActiveToken(true);
                    _quickItemInfo[i].SetData(item,count);
                    break;
                }
            }
        }
        else //소비아이템
        {
            for (int i = usableSlotRange[0]; i <= usableSlotRange[1]; i++)
            {
                if (!_quickItemInfo[i].GetActiveToken())
                {
                    _quickItemInfo[i].SetActiveToken(true);
                    _quickItemInfo[i].SetData(item, count);
                    break;
                }
            }
        }
    }
}
