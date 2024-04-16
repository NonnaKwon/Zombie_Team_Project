using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Item
{

    public override void UseItem()
    {
        StatusController sc = Manager.Game.Player.GetComponent<StatusController>();
        sc.ChangeData(Define.Status.Fatigue, 0.3f, true);
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Items/coffee");
    }
}
