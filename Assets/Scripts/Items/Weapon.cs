using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : Item
{
    [SerializeField] protected WeaponData _data;

    protected Animator _playerAnimator;
    protected bool _sGet = false;

    public bool OnAttack { get; set; } = false;
    public float AttackSpeed { get; set; }
    public float AttackSpeedBase { get { return _data.attackSpeed; } }

    protected virtual void Awake()
    {
        _playerAnimator = GetComponentInParent<Animator>();
        AttackSpeed = AttackSpeedBase;
    }

    protected virtual void OnEnable()
    {
        Manager.Game.Player.ChangeAnimationLayer(_data.animationLayer);
    }

    protected virtual void OnDisable()
    {
        Manager.Game.Player.ChangeAnimationLayer("Base Layer");
    }

    public abstract void Attack();

}
