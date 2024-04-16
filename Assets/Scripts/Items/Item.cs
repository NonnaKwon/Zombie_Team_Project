using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Item
{
    protected ItemData _data;
    public ItemData Data { get { return _data; } }

    public abstract void SetData();
    public abstract void UseItem();

}
