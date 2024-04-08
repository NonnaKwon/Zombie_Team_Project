using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData _idata;
    protected PlayerController _player;

    protected virtual void Awake()
    {
        _player = Manager.Game.Player;
    }
    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    public abstract void UseItem();

}
