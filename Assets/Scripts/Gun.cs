using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public override void Attack()
    {
        float damage = Random.Range(_data.minDamage, _data.maxDamage);
        Debug.Log("ÃÑ °ø°Ý! : " + damage);
        _playerAnimator.Play("fire");
    }
}
