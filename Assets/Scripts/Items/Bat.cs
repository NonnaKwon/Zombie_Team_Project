using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bat : Weapon
{
    [SerializeField] AttackPoint _attackPoint;

    Collider[] _colliders = new Collider[100];

    protected override void Awake()
    {
        base.Awake();
        AttackSpeed = _data.attackSpeed;
    }

    public override void Attack()
    {
        OnAttack = true;
        _playerAnimator.Play("hit");
        _playerAnimator.SetFloat("attackSpeed", AttackSpeed);
        StartCoroutine(CoHit());
    }

    IEnumerator CoHit()
    {
        yield return new WaitForSeconds(0.2f / AttackSpeed);
        Hit();
    }

    private void Hit()
    {
        float damage = Random.Range(_data.minDamage, _data.maxDamage);
        _attackPoint.Hit(damage);
        OnAttack = false;
    }

}
