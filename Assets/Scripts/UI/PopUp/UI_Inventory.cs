using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : PopUpUI           
{
    private Inventory _inventory;
    enum GameObjects
    {
        Items
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        Debug.Log("�κ��丮 ����");
    }


    private void OnEnable()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {

    }

}
