using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieController : MonoBehaviour, IDamagable
{
    [SerializeField] ZombieType type;
    [SerializeField] AttackPoint attackPoint;
    [SerializeField] float attackDamage;

    public Transform player;
    public Rigidbody rigid;
    public Animator animator;

    public float sightRange, attackRange;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float zombieSpeed;
    private float time = 0;
    private float releaseDis = 150f;
    public float MoveSpeed { get { return zombieSpeed; } }
    private PooledObject bloodEffect;

    private ZombieState currentState;
    public float hp = 100;
    PooledObject coin;


    private enum ZombieState
    {
        Idle,
        Chase,
        Attack
    }

    enum ZombieType
    {
        walk,
        run,
        crawl
    }
    private void Start()
    {
        bloodEffect = Manager.Resource.Load<PooledObject>("Prefabs/Effects/BloodEffect");
        coin = Manager.Resource.Load<PooledObject>("Prefabs/GoldCoins");

    }

    private void Awake()
    {
        player = Manager.Game.Player.gameObject.transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
        animator.SetInteger("ZombieType", (int)type);
        attackPoint = GetComponentInChildren<AttackPoint>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                LookForPlayer();
                break;
            case ZombieState.Chase:
                ChasePlayer();
                break;
            case ZombieState.Attack:
                AttackPlayer();
                break;
        }
    }

    private void LookForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= sightRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
        }
        else if (distanceToPlayer >= releaseDis)
            GetComponent<PooledObject>().Release();
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = ZombieState.Attack;
        }
        else if (distanceToPlayer > sightRange)
        {
            currentState = ZombieState.Idle;
            animator.SetBool("IsChase", false);
        }
        else
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * zombieSpeed * Time.deltaTime;
            rigid.MovePosition(newPosition);

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    private void AttackPlayer()
    {
        time += Time.deltaTime;
        if (time > timeBetweenAttacks)
        {
            time = 0;
            attackPoint.Hit(attackDamage);
            if (ZombieType.crawl == type)
                animator.Play("Bite");
            else
                animator.Play("Attack");
        }

        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
        }
    }


    IEnumerator CoDie()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSecondsRealtime(1.5f);
        DropItem();
        GetComponent<PooledObject>().Release();
    }

    private void DropItem()
    {
        Manager.Pool.GetPool(coin, transform.position, transform.rotation);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        Manager.Pool.GetPool(bloodEffect, transform.position + new Vector3(0, 1.5f, 0),transform.rotation);

        if (hp <= 0)
        {
            StartCoroutine(CoDie());
            Debug.Log("Á»ºñ Á×À½");
        }
    }

}
