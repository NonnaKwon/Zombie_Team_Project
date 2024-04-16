using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatItem : Item
{
    public override void UseItem()
    {
        Manager.Game.Player.GetComponent<FightController>().SetWeapon(this);
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Bat1");
    }

}
