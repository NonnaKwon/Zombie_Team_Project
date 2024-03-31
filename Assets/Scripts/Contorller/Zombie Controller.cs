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
    public Transform player; // 플레이어 위치 참조
    public LayerMask whatIsGround, whatIsPlayer; // 바닥과 플레이어를 구분하기 위한 레이어마스크
    public enum StateMachine { Idle, Patrol, Chase, Attack } // 현재 좀비 상태를 나타내는 열거형
    public StateMachine statemachine; //현재 좀비 상태
    public bool isAlive;
    public bool playerInSightRange, playerInAttackRange;
    public Vector3 walkPoint;
    public bool walkPointSet;
    private bool alreadyAttacked; // 이미 공격했는지 여부

    public float sightRange, attackRange;
    public float speed;
    public float patrolSpeed;
    public float chaseSpeed;
    public float sightrange;
    public float attackrange;
    public float timeBetweenAttacks;
    public float walkPointRange;
    public int health = 100; // 좀비의 초기 생명력
    public GameObject[] itemsToDrop; // 드랍할 수 있는 아이템들의 배열
    public float dropRadius = 0.5f; // 아이템을 드랍할 때 좀비 위치에서의 최대 반경

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isAlive)
        {
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        switch (statemachine)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Chase:
                Chase();
                break;
            case StateMachine.Attack:
                Attack();
                break;
        }
    }

    private void Patrol() // 좀비 순찰 기능
    {
        agent.speed = patrolSpeed;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        // 순찰 지점에 도착했다면 다음 순찰 지점 찾기
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        // 플레이어가 시야 범위 내에 있다면 추적 상태로 전환
        if (Physics.CheckSphere(transform.position, sightrange, whatIsPlayer))
        {
            statemachine = StateMachine.Chase;
        }
    }

    private void SearchWalkPoint()
    {
        // 랜덤으로 순찰 지점 결정
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround));

        if (Physics.Raycast(walkPoint, -Vector3.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
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
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        if (!Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
        {
            statemachine = StateMachine.Chase;
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // 받은 피해만큼 생명력을 감소시킴

        if (health <= 0) // 생명력이 0 이하가 되면 죽음
        {
            Die();
        }
    }

    private void Die()
    {
        DropRandomItem(); // 무작위 아이템 드랍
        Destroy(gameObject); // 좀비 게임오브젝트를 파괴하여 죽음 처리
    }

    private void DropRandomItem()
    {
        if (itemsToDrop.Length > 0) // 드랍할 아이템이 설정되어 있다면
        {
            int randomIndex = Random.Range(0, itemsToDrop.Length); // 무작위 인덱스 선택
            Vector3 dropPosition = transform.position + Random.insideUnitSphere * dropRadius; // 드랍 위치 결정
            dropPosition.y = transform.position.y; // y 위치는 조정하지 않음
            Instantiate(itemsToDrop[randomIndex], dropPosition, Quaternion.identity); // 아이템 생성
        }
    }
}
