using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public Transform player;
    public Rigidbody rigid;
    public Animator animator;

    public float sightRange, attackRange;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float zombieSpeed;

    private ZombieState currentState;
    public int health = 100;
    public GameObject[] dropItems;

    private enum ZombieState
    {
        Idle,
        Chase,
        Attack
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
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
        }
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
        if (!alreadyAttacked)
        {
            // Implement attack logic here
            Debug.Log("Zombie attacks the player!");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            currentState = ZombieState.Chase;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        DropItem();
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (dropItems.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
