using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_QuickSlot : BaseUI
{
    Inventory _inventory;
    List<UI_QuickToken> _quickItemInfo;
    Item[] _items = { null, null, null, null, null, null, null };
    int[] usableSlotRange = { 3, 7 };
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
        if(weapon != null) //���������
        {
            int index = 0;
            if (weapon.ItemName.Equals("Gun"))
                index = 0;
            else if (weapon.ItemName.Equals("Bat"))
                index = 1;
            else
                index = 2;
            _items[index] = FindItemClass(item);
            _quickItemInfo[index].SetActiveToken(true);
            _quickItemInfo[index].SetData(item, count);
        }
        else //�Һ������
        {
            for (int i = usableSlotRange[0]; i <= usableSlotRange[1]; i++)
            {
                if (!_quickItemInfo[i].GetActiveToken())
                {
                    _items[i] = FindItemClass(item);
                    _quickItemInfo[i].SetActiveToken(true);
                    _quickItemInfo[i].SetData(item, count);
                    break;
                }
            }
        }
    }

    private void OnOne(InputValue value)
    {
        UseItem(0);
    }

    private void OnTwo(InputValue value)
    {
        UseItem(1);
    }

    private void OnThree(InputValue value)
    {
        UseItem(2);
    }

    private void OnFour(InputValue value)
    {
        UseItem(3);
    }

    private void OnFive(InputValue value)
    {
        UseItem(4);
    }

    private void OnSix(InputValue value)
    {
        UseItem(5);
    }

    private void OnSeven(InputValue value)
    {
        UseItem(6);
    }

    private void UseItem(int index)
    {
        _items[index].SetData();
        _items[index].UseItem();
        _inventory.RemoveItem(_items[index].Data);
        _quickItemInfo[index].DecreaseCount();
    }

    private Item FindItemClass(ItemData data)
    {
        switch(data.ItemName)
        {
            case "Gun":
                return new GunItem();
            case "Bat":
                return new BatItem();
            case "Grenade":
                return new Grenade();
            case "water":
                return new Water();
            case "bullet":
                return new Bullet();
            case "energybar":
                return new EnergyBar();
            case "can":
                return new CannedFood();
            case "coffee":
                return new Coffee();
            default:
                return null;
        }
    }

}
