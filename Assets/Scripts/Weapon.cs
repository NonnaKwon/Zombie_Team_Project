using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : Item
{
    [SerializeField] protected WeaponData _data;
    protected Animator _playerAnimator;
    protected bool _sGet = false;
    

    protected virtual void Awake()
    {
        _playerAnimator = GetComponentInParent<Animator>();
        Debug.Log(_playerAnimator);
    }

    protected void OnEnable()
    {
        Debug.Log(Manager.Game.Player);
        Manager.Game.Player.ChangeAnimationLayer(_data.animationLayer);
    }

    protected void OnDisable()
    {
        Manager.Game.Player.ChangeAnimationLayer("Base Layer");
    }

    public abstract void Attack();

}
