using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour, IDamagable
{
    public Transform player;
    public Rigidbody rigid;
    public Animator animator;

    public enum StateMachine { Idle, Chase, Attack, Die }
    public StateMachine statemachine = StateMachine.Idle;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float sightRange, attackRange;
    public float zombieSpeed;
    public bool alreadyAttacked;
    public float timeBetweenAttacks;

    public int hp;
    public GameObject[] itemsToDrop;
    public float dropRadius = 0.5f;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 플레이어와 좀비 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= sightRange && distanceToPlayer > attackRange)
        {
            statemachine = StateMachine.Chase;
        }
        else if (distanceToPlayer <= attackRange)
        {
            statemachine = StateMachine.Attack;
        }

        switch (statemachine)
        {
            case StateMachine.Chase:
                Chase();
                break;
            case StateMachine.Attack:
                Attack();
                break;
        }

    }

    private void Chase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        rigid.MovePosition(transform.position + direction * zombieSpeed * Time.fixedDeltaTime);
    }

    private void Attack()
    {
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Debug.Log("플레이어 공격");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        hp -= (int)damage;

        if (hp <= 0)
        {
            Die();
            Debug.Log("좀비 사망");
        }
    }

    private void Die()
    {
        DropRandomItem();
        Destroy(gameObject);
    }

    private void DropRandomItem()
    {
        if (itemsToDrop.Length > 0)
        {
            int randomIndex = Random.Range(0, itemsToDrop.Length);
            Vector3 dropPosition = transform.position + Random.insideUnitSphere * dropRadius;
            dropPosition.y = transform.position.y;
            Instantiate(itemsToDrop[randomIndex], dropPosition, Quaternion.identity);
        }
    }
}
