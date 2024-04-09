using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
{
    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Grenade");
    }

}
