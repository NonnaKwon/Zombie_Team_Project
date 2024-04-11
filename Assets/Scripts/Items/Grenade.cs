using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
{
    public override void UseItem()
    {
        Manager.Game.Player.GetComponent<FightController>().ThrowGrenade();
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Grenade");
    }

}
