using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class FightController : MonoBehaviour, IDamagable
{
    [SerializeField] Weapon[] _weapons;
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
    Weapon _curWeapon;
    GrenadeObject _grenadePrefab;


    private void Awake()
    {
        _hp = 100;
    }

    private void Start()
    {
        Manager.Game.GameUI.SetMaxHP(_hp);
        _uiInventory = Manager.Game.GameUI.GetComponentInChildren<UI_Inventory>();
        _uiInventory.SetInit();
        _inventory = GetComponent<Inventory>();
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<PlayerController>();
        _grenadePrefab = Manager.Resource.Load<GrenadeObject>("Prefabs/Weapon/Grenade");
    }

    private void OnAttack(InputValue value)
    {
        if (!_player.CanMove)
            return;
        if (_curWeapon != null && !_curWeapon.OnAttack)
            _curWeapon.Attack();
    }
    
    public void SetWeapon(Item item)
    {
        if(_curWeapon != null)
        {
            if (_curWeapon._data == item.Data)
                return;
            _curWeapon.gameObject.SetActive(false);
            _curWeapon = null;
        }

        for(int i=0;i<_weapons.Length;i++)
        {
            if (item.Data == _weapons[i]._data)
            {
                _curWeapon = _weapons[i];
                _weapons[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    public void ThrowGrenade()
    {
        _animator.Play("Grenade");
        StartCoroutine(CoThrow());
    }

    IEnumerator CoThrow()
    {
        yield return new WaitForSeconds(1.3f);
        Vector3 pos = transform.position + transform.forward * 2f + Vector3.up * 2.3f;
        GrenadeObject grenade = Instantiate(_grenadePrefab, pos, transform.rotation);
        grenade.ForwardForce(transform.forward * 100f);
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        Manager.Game.GameUI.ChangeHP(_hp);
        Debug.Log("Player : 공격받음");
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
