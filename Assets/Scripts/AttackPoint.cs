using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] float _attackRange;
    [SerializeField] LayerMask _mask;
    Collider[] _colliders = new Collider[100];

    public void Hit(float damage)
    {
        int size = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _colliders, _mask);
        for (int i = 0; i < size; i++)
        {
            IDamagable damagable = _colliders[i].gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    public void Hit(float minDamage,float maxDamage)
    {
        int size = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _colliders, _mask);
        for (int i = 0; i < size; i++)
        {
            IDamagable damagable = _colliders[i].gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(Random.Range(minDamage, maxDamage));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    public void SetRange(float range)
    {
        _attackRange = range;
    }
}
