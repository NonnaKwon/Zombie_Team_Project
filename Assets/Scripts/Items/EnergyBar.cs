using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : Item
{
    public override void UseItem()
    {
        StatusController sc = Manager.Game.Player.GetComponent<StatusController>();
        sc.ChangeData(Define.Status.Stamina, 0.3f, true);
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Items/energyBar");
    }
}
