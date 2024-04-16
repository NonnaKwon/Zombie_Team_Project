using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bat : Weapon
{
    [SerializeField] AttackPoint _attackPoint;

    Collider[] _colliders = new Collider[100];

    public AudioClip attackSound;
    public float soundDelay = 0.3f;

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
        StartCoroutine(DelaySound(soundDelay));
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
        //SoundManager.instance.PlayMeleeHitSound();
        OnAttack = false;
    }

    private IEnumerator DelaySound(float delay)
    {
        yield return new WaitForSeconds(delay); // 딜레이
        SoundManager.Instance.PlaySFX(attackSound); // 딜레이 후 사운드 재생
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie") || collision.gameObject.CompareTag("BossZombie"))
        {
            SoundManager.instance.PlayMeleeHitSound();
        }
    }

}
