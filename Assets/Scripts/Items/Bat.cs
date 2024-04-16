using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bat : Weapon
{
    [SerializeField] AttackPoint _attackPoint;

    Collider[] _colliders = new Collider[100];
    AudioClip _audio;

    public AudioClip attackSound;
    public float soundDelay = 0.3f;

    protected override void Awake()
    {
        base.Awake();
        _audio = Manager.Resource.Load<AudioClip>("Sounds/BatAttack");
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
        _attackPoint.Hit(damage,true);
        Manager.Sound.PlaySFX(_audio);
        OnAttack = false;
    }


}
