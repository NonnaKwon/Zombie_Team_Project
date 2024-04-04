using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightController : MonoBehaviour, IDamagable
{
    [SerializeField] Weapon _curWeapon;

    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    private float _hp;
    private float _power;
    private float _range;
    private float _attackSpeed;

    Inventory _inventory;
    Animator _animator;

    private void Awake()
    {

    }

    private void Start()
    {
        _curWeapon.gameObject.SetActive(true);
    }

    private void OnAttack(InputValue value)
    {
        if(_curWeapon != null)
            _curWeapon.Attack();
    }

    public void TakeDamage(float damage)
    {

    }
}
