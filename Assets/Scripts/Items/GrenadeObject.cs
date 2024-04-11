using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    WeaponData _data;
    Rigidbody _rigid;
    float _power;
    AttackPoint _attackPoint;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _attackPoint = GetComponentInChildren<AttackPoint>();
        _data = Manager.Resource.Load<WeaponData>("Data/Grenade");
        _power = 7f;
    }
    private void OnTriggerEnter(Collider other)
    {
        MapController map = other.GetComponent<MapController>();
        if (map != null)
            StartCoroutine(Bang());
    }

    public void ForwardForce(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        _rigid.velocity = new Vector3(0, _power, 0) + dir * _power;
    }


    IEnumerator Bang()
    {
        _attackPoint.SetRange(_data.attackRange);
        _attackPoint.Hit(_data.minDamage, _data.maxDamage);
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }
}
