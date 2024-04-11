using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannedFood : Item
{
    public override void UseItem()
    {
        StatusController sc = Manager.Game.Player.GetComponent<StatusController>();
        sc.ChangeData(Define.Status.Hunger, 0.3f, true);
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Items/cannedFood");
    }
}
