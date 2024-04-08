using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class FightController : MonoBehaviour, IDamagable
{
    [SerializeField] Weapon _curWeapon;

    public float AttackSpeed { set { _curWeapon.AttackSpeed = value; } }
    public float AttackSpeedBase { get { return _curWeapon.AttackSpeedBase; } }

    private float _hp;
    private float _power;
    private float _range;
    private float _attackSpeed;

    UI_Inventory _uiInventory;
    Inventory _inventory;
    Animator _animator;
    PlayerController _player;

    private void Awake()
    {
        _hp = 100;
    }

    private void Start()
    {
        _curWeapon.gameObject.SetActive(true);
        Manager.Game.GameUI.SetMaxHP(_hp);

        _uiInventory = Manager.Game.GameUI.GetComponentInChildren<UI_Inventory>();
        _inventory = GetComponent<Inventory>();
        _uiInventory.SetInventory(_inventory);
        _uiInventory.gameObject.SetActive(false);

        _player = GetComponent<PlayerController>();
    }

    private void OnAttack(InputValue value)
    {
        if (!_player.CanMove)
            return;
        if (_curWeapon != null && !_curWeapon.OnAttack)
            _curWeapon.Attack();
    }
    
    public void TakeDamage(float damage)
    {
        _hp -= damage;
        Manager.Game.GameUI.ChangeHP(_hp);
        if (_hp <= 0)
        {
            _player.StateMachine.ChangeState(Define.PlayerState.Die);
        }
    }

    private void OnInventory(InputValue value)
    {
        if (!_uiInventory.gameObject.activeSelf)
        {
            _uiInventory.gameObject.SetActive(true);
            _player.StateMachine.ChangeState(PlayerState.Interact);
        }
        else
        {
            _uiInventory.gameObject.SetActive(false);
            _player.StateMachine.ChangeState(PlayerState.Idle);
        }
    }
}
