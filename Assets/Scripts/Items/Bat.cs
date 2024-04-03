using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bat : Weapon
{
    [SerializeField] GameObject _attackPoint;
    [SerializeField] LayerMask _mask;

    private float _attackRange = 5f;

    Collider[] _colliders = new Collider[100];

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void Attack()
    {
        Debug.Log("배트 공격");
        _playerAnimator.Play("hit");
        StartCoroutine(CoHit());
    }

    IEnumerator CoHit()
    {
        yield return new WaitForSeconds(0.2f);
        Hit();
    }

    private void Hit()
    {
        int size = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _colliders, _mask);
        Debug.Log(size);
        for (int i = 0; i < size; i++)
        {
            IDamagable damagable = _colliders[i].gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Debug.Log("공격함");
            }
        }
    }
}
