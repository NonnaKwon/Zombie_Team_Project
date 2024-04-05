using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightController : MonoBehaviour, IDamagable
{
    [SerializeField] Weapon _curWeapon;

    public float AttackSpeed { set { _curWeapon.AttackSpeed = value; } }
    public float AttackSpeedBase { get { return _curWeapon.AttackSpeedBase; } }

    private float _hp;
    private float _power;
    private float _range;
    private float _attackSpeed;

    Inventory _inventory;
    Animator _animator;

    private void Awake()
    {
        _hp = 100;
    }

    private void Start()
    {
        _curWeapon.gameObject.SetActive(true);
        Manager.Game.GameUI.SetMaxHP(_hp);
    }

    private void OnAttack(InputValue value)
    {
        if (!Manager.Game.Player.CanMove)
            return;
        if (_curWeapon != null && !_curWeapon.OnAttack)
            _curWeapon.Attack();
    }
    
    public void TakeDamage(float damage)
    {
        _hp -= damage;
        Manager.Game.GameUI.ChangeHP(_hp);
        Debug.Log("Player : 공격받음");
        if (_hp <= 0)
        {
            Manager.Game.Player.StateMachine.ChangeState(Define.PlayerState.Die);
        }
    }
}
