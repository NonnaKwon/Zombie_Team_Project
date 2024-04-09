using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossZombie : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackDamage;
    public int maxHp = 5000;
    public int curHp;
    public float moveSpeed;
    public float attackRange;
    public float sightRange;
    public Rigidbody rigid;
    public Animator animator;
    public GameObject[] dropItem;
    public GameObject fireBloodPrefab;
    public GameObject zombiePrefab;
    private Transform player;
    public Transform[] createPoint;

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;

    void Start()
    {
        curHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(PhaseManager());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= sightRange)
        {
            // �÷��̾ ���� ����
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
                CancelInvoke("SummonZombies"); // 3����� �Ǹ� ��ȯ �ߴ�
            }
            else if (curHp < maxHp * 0.7 && currentPhase == BossPhase.Phase1)
            {
                currentPhase = BossPhase.Phase2;
                CreateZombies(); // 2������� �� ���� ���� 200���� ��ȯ
            }

            yield return new WaitForSeconds(5f); // 5�ʸ��� üũ
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
                    animator.SetTrigger("MeleeAttack");
                }
                break;
            case BossPhase.Phase2:
                break;
            case BossPhase.Phase3:
                // 3������: ���Ÿ� ����. �÷��̾ �� �ָ� ���� ��� ���Ÿ� ����
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
            // ��ȯ ��ġ�� �����ϰ� �����ϱ� ���� CreatePoint �� �ϳ��� �������� ����
            Transform summonPoint = createPoint[Random.Range(0, createPoint.Length)];
            Instantiate(zombiePrefab, summonPoint.position, Quaternion.identity);
        }
    }

    void FireBlood()
    {
        Instantiate(fireBloodPrefab, attackPoint.position, Quaternion.LookRotation(player.position - attackPoint.position));
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        DropItem();
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (dropItem.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItem.Length);
            Instantiate(dropItem[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
