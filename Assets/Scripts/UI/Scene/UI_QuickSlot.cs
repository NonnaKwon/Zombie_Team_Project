using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_QuickSlot : BaseUI
{
    Inventory _inventory;
    List<UI_QuickToken> _quickItemInfo;
    Item[] _items = { null, null, null, null, null, null, null };
    ItemData _gunData;
    int[] usableSlotRange = { 3, 6 };
    enum Gameobjects
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _quickItemInfo = GetComponentsInChildren<UI_QuickToken>().ToList();
        foreach (UI_QuickToken token in _quickItemInfo)
        {
            token.SetActiveToken(false);
        }
        _inventory = Manager.Game.Player.GetComponent<Inventory>();
        _inventory.BulletChange -= BulletChange;
        _inventory.BulletChange += BulletChange;
    }

    public void AddQuickSlot(ItemData item,int count)
    {
        WeaponData weapon = item as WeaponData;
        if(weapon != null) //무기아이템
        {
            int index = 0;
            if (weapon.ItemName.Equals("Gun"))
            {
                index = 0;
                _gunData = item;
                count = _inventory.BulletCount;
            }
            else if (weapon.ItemName.Equals("Bat"))
                index = 1;
            else
                index = 2;
            _items[index] = FindItemClass(item);
            _items[index].SetData();
            _quickItemInfo[index].SetActiveToken(true);
            _quickItemInfo[index].SetData(item, count);
        }
        else //소비아이템
        {
            if (item.ItemName.Equals("bullet"))
                return;
            for (int i = usableSlotRange[0]; i <= usableSlotRange[1]; i++)
            {
                if (!_quickItemInfo[i].GetActiveToken())
                {
                    _items[i] = FindItemClass(item);
                    _items[i].SetData();
                    _quickItemInfo[i].SetActiveToken(true);
                    _quickItemInfo[i].SetData(item, count);
                    break;
                }
            }
        }
    }

    public void PlusCountQuickSort(ItemData data ,int count)
    {
        WeaponData weapon = data as WeaponData;
        if (weapon != null)
            return;
        //겹치는 버그 해결 -> 파밍이 되면, 숫자 업데이트
        for (int i = usableSlotRange[0]; i <= usableSlotRange[1]; i++)
        {
            if (_items[i] != null && _items[i].Data.ItemName.Equals(data.ItemName))
            {
                _quickItemInfo[i].IncreaseCount(count);
            }

        }
    }

    public void BulletChange()
    {
        if (_gunData != null)
        {
            _quickItemInfo[0].SetData(_gunData, _inventory.BulletCount);
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
        if (_items[index] == null)
            return;
        _items[index].UseItem();
        if(usableSlotRange[0]-1 <= index && index <= usableSlotRange[1]-1) //수류탄~소비아이템만
        {
            _inventory.RemoveItem(_items[index].Data);
            int leftCount = _quickItemInfo[index].DecreaseCount();
            if (leftCount == 0)
                _items[index] = null;
        }
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
