using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightController : MonoBehaviour
{
    Inventory _inventory;
    [SerializeField] Weapon _curWeapon;

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
}
