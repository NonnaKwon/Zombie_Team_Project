using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bat : Weapon
{
    [SerializeField] GameObject _attackPoint;
    [SerializeField] LayerMask _mask;

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
        int size = Physics.OverlapSphereNonAlloc(transform.position, _data.attackRange, _colliders, _mask);
        Debug.Log(size);
        for (int i = 0; i < size; i++)
        {
            IDamagable damagable = _colliders[i].gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                //몬스터 공격
                float damage = Random.Range(_data.minDamage, _data.maxDamage);
                damagable.TakeDamage(damage);
                Debug.Log("Zombie : 공격당함");
            }
        }
        OnAttack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.attackRange);
    }
}
