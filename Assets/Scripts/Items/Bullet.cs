using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }

    public override void SetData()
    {
        _data = Manager.Resource.Load<ItemData>("Data/Items/bullets");
    }
}