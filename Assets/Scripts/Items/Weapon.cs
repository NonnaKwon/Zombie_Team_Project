using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Weapon : Item
{
    protected WeaponData _data;
    protected Animator _playerAnimator;
    protected bool _isGet = false;

    public bool OnAttack { get; set; } = false;
    public float AttackSpeed { get; set; }
    public float AttackSpeedBase { get { return _data.attackSpeed; } }

    protected override void Awake()
    {
        base.Awake();
        _data = _idata as WeaponData;
        if (_data == null)
            Debug.Log("무기 데이터 null");
        _playerAnimator = GetComponentInParent<Animator>();
        AttackSpeed = AttackSpeedBase;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _player.ChangeAnimationLayer(_data.animationLayer);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _player.ChangeAnimationLayer("Base Layer");
    }

    public abstract void Attack();
}
