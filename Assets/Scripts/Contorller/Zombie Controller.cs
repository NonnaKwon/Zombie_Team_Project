using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public enum StateMachine { Idle, Patrol, Chase, Attack } // 좀비 상태
    public StateMachine statemachine;
    public bool isAlive;
    public bool playerInSightRange, playerInAttackRange;
    public Vector3 walkPoint;
    public bool walkPointSet;

    public float sightRange, attackRange;
    public float speed;
    public float patrolSpeed;
    public float chaseSpeed;
    public float sightrange;
    public float attackrange;
    public float walkPointRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isAlive) { return; }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsGround);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    }

    private void Patrol() // 좀비 순찰 기능
    {
        agent.speed = patrolSpeed;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        if (Physics.CheckSphere(transform.position, sightrange, whatIsGround))
        {
            statemachine = StateMachine.Chase;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround));
        walkPointSet = true;
        
    }

    private void Chase() // 좀비 추격 기능
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
        {
            statemachine = StateMachine.Attack;
        }
    }

    private void Attack() // 좀비 공격 기능
    {

    }
}
