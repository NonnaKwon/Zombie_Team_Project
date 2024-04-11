using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.Animations.Rigging;

public class BossZombie : MonoBehaviour
{
    public int maxHp = 5000;
    private int curHp;
    public float attackRange = 1.5f;
    public float sightRange = 15.0f; // �÷��̾� ���� ����
    public float zombieSpeed;
    public GameObject FireBloodPrefab; // ���Ÿ� ���ݿ� ������Ʈ
    public Transform attackPoint; // ���� �߻� ��ġ
    public GameObject zombiePrefab; // ��ȯ�� ���� ������
    public Transform[] CreatePoints; // ���� ��ȯ ��ġ
    private Transform player;
    private Animator animator;
    private Rigidbody rigid;
    [SerializeField] AttackPoint attackPoints;
    [SerializeField] ZombieType type;

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;
    private ZombieState currentState;
    private enum ZombieState
    {
        Idle,
        Chase,
        Attack
    }
    enum ZombieType
    {
        Boss
    }
    void Start()
    {
        curHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(PhaseManager());
    }
    private void Awake()
    {
        player = Manager.Game.Player.gameObject.transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
        animator.SetInteger("ZombieType", (int)type);
        attackPoints = GetComponentInChildren<AttackPoint>();
    }
    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                LookForPlayer();
                break;
            case ZombieState.Chase:
                ChasePlayer();
                break;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= sightRange)
        {
            AttackPlayer(distanceToPlayer);
        }
    }

    IEnumerator PhaseManager()
    {
        while (curHp > 0)
        {
            if (curHp < maxHp * 0.3)
            {
                currentPhase = BossPhase.Phase3;
                CancelInvoke("CreateZombies"); // 3����� �Ǹ� ��ȯ �ߴ�
            }
            else if (curHp < maxHp * 0.7 && currentPhase == BossPhase.Phase1)
            {
                currentPhase = BossPhase.Phase2;
                CreateZombies(); // 2������� �� ���� ���� 200���� ��ȯ
            }

            yield return new WaitForSeconds(5f); // 5�ʸ��� üũ
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

    void AttackPlayer(float distanceToPlayer)
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                // 1������: �÷��̾ ���� ���� ���� ���� ���� ��� ���� ���� ����
                if (distanceToPlayer <= attackRange)
                {
                    animator.SetTrigger("IsAttack");
                    animator.Play("IsAttack");
                }
                break;
            case BossPhase.Phase2:
                break;
            case BossPhase.Phase3:
                // 3������: ���Ÿ� ���� ����. �÷��̾ �� �ָ� ���� ��� ���Ÿ� ������ ���
                if (!IsInvoking("FireBlood"))
                {
                    InvokeRepeating("FireBlood", 0f, 2f); // 2�ʸ��� ���� �߻�
                }
                break;
        }
    }

    void CreateZombies()
    {
        for (int i = 0; i < 200; i++) // 200���� ��ȯ
        {
            // ��ȯ ��ġ�� �����ϰ� �����ϱ� ���� CreatePoints �� �ϳ��� �������� ����
            Transform summonPoint = CreatePoints[Random.Range(0, CreatePoints.Length)];
            Instantiate(zombiePrefab, summonPoint.position, Quaternion.identity);
        }
    }

    void FireBlood()
    {
        Instantiate(FireBloodPrefab, attackPoint.position, Quaternion.LookRotation(player.position - attackPoint.position));
    }

    public void MeleeAttackDamage()
    {
        
    }
}
