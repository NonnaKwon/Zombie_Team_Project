using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public Transform player;
    public Rigidbody rigid;
    public LayerMask whatIsGround, whatIsPlayer;

    public enum StateMachine { Idle, Chase, Attack }
    public StateMachine statemachine = StateMachine.Idle;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float sightRange, attackRange;
    public float zombieSpeed;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

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
                ChasePlayer();
                break;
            case StateMachine.Attack:
                AttackPlayer();
                break;
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        rigid.MovePosition(transform.position + direction * zombieSpeed * Time.fixedDeltaTime);
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            
            // player.TakeDamage(damage);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
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

    private void Respawn()
    {

    }

}
