using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Item
{

    public override void UseItem()
    {

    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Items/coffee");
    }
}
