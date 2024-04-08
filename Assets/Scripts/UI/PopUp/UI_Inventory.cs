using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inventory : PopUpUI           
{

    private Inventory _inventory;
    private int count = 0;
    enum GameObjects
    {
        Items,
        Cash
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        Debug.Log("인벤토리 참조");
    }


    private void OnEnable()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        GetUI<TMP_Text>(GameObjects.Cash.ToString()).text = Manager.Game.Player.Coin.ToString();

    }

}
