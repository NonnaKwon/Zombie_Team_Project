using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightController : MonoBehaviour
{
    [SerializeField] Weapon _curWeapon;
    Inventory _inventory;

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
