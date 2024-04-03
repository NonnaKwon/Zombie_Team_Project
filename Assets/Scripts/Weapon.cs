using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : Item
{
    [SerializeField] protected WeaponData _data;
    protected Animator _playerAnimator;
    protected bool _sGet = false;

    protected void Awake()
    {
        _playerAnimator = GetComponentInParent<Animator>();
    }

    protected void OnEnable()
    {
        Manager.Game.Player.ChangeAnimationLayer(_data.animationLayer);
    }

    private void OnDisable()
    {
        Manager.Game.Player.ChangeAnimationLayer("Base Layer");
    }

    public abstract void Attack();

}
