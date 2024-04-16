using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    public WeaponData _data;
    protected Animator _playerAnimator;
    protected PlayerController _player;
    protected bool _isGet = false;

    public bool OnAttack { get; set; } = false;
    public float AttackSpeed { get; set; }
    public float AttackSpeedBase { get { return _data.attackSpeed; } }

    protected virtual void Awake()
    {
        _playerAnimator = GetComponentInParent<Animator>();
        _player = Manager.Game.Player;
        AttackSpeed = AttackSpeedBase;
    }

    protected virtual void OnEnable()
    {
        _player.ChangeAnimationLayer(_data.animationLayer);
    }

    protected virtual void OnDisable()
    {
        _player.ChangeAnimationLayer("Base Layer");
    }

    public abstract void Attack();
}
