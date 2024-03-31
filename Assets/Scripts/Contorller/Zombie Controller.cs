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
    public Transform player; // �÷��̾� ��ġ ����
    public LayerMask whatIsGround, whatIsPlayer; // �ٴڰ� �÷��̾ �����ϱ� ���� ���̾��ũ
    public enum StateMachine { Idle, Patrol, Chase, Attack } // ���� ���� ���¸� ��Ÿ���� ������
    public StateMachine statemachine; //���� ���� ����
    public bool isAlive;
    public bool playerInSightRange, playerInAttackRange;
    public Vector3 walkPoint;
    public bool walkPointSet;
    private bool alreadyAttacked; // �̹� �����ߴ��� ����

    public float sightRange, attackRange;
    public float speed;
    public float patrolSpeed;
    public float chaseSpeed;
    public float sightrange;
    public float attackrange;
    public float timeBetweenAttacks;
    public float walkPointRange;
    public int health = 100; // ������ �ʱ� �����
    public GameObject[] itemsToDrop; // ����� �� �ִ� �����۵��� �迭
    public float dropRadius = 0.5f; // �������� ����� �� ���� ��ġ������ �ִ� �ݰ�

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

    private void Patrol() // ���� ���� ���
    {
        agent.speed = patrolSpeed;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        // ���� ������ �����ߴٸ� ���� ���� ���� ã��
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        // �÷��̾ �þ� ���� ���� �ִٸ� ���� ���·� ��ȯ
        if (Physics.CheckSphere(transform.position, sightrange, whatIsPlayer))
        {
            statemachine = StateMachine.Chase;
        }
    }

    private void SearchWalkPoint()
    {
        // �������� ���� ���� ����
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround));

        if (Physics.Raycast(walkPoint, -Vector3.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void Chase() // ���� �߰� ���
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
        {
            statemachine = StateMachine.Attack;
        }
    }

    private void Attack() // ���� ���� ���
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
        health -= damage; // ���� ���ظ�ŭ ������� ���ҽ�Ŵ

        if (health <= 0) // ������� 0 ���ϰ� �Ǹ� ����
        {
            Die();
        }
    }

    private void Die()
    {
        DropRandomItem(); // ������ ������ ���
        Destroy(gameObject); // ���� ���ӿ�����Ʈ�� �ı��Ͽ� ���� ó��
    }

    private void DropRandomItem()
    {
        if (itemsToDrop.Length > 0) // ����� �������� �����Ǿ� �ִٸ�
        {
            int randomIndex = Random.Range(0, itemsToDrop.Length); // ������ �ε��� ����
            Vector3 dropPosition = transform.position + Random.insideUnitSphere * dropRadius; // ��� ��ġ ����
            dropPosition.y = transform.position.y; // y ��ġ�� �������� ����
            Instantiate(itemsToDrop[randomIndex], dropPosition, Quaternion.identity); // ������ ����
        }
    }
}
