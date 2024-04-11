using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombie : MonoBehaviour
{
    public int maxHp = 5000;
    private int curHp;
    public float meleeAttackRange = 1.5f;
    public float noticeRange = 15.0f; // �÷��̾� ���� ����
    public GameObject projectilePrefab; // ���Ÿ� ���ݿ� ������Ʈ
    public Transform attackPoint; // ���� �߻� ��ġ
    public GameObject zombiePrefab; // ��ȯ�� ���� ������
    public Transform[] CreatePoints; // ���� ��ȯ ��ġ
    private Transform player;
    private Animator animator;

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
        if (distanceToPlayer <= noticeRange)
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

    void AttackPlayer(float distanceToPlayer)
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                // 1������: �÷��̾ ���� ���� ���� ���� ���� ��� ���� ���� ����
                if (distanceToPlayer <= meleeAttackRange)
                {
                    animator.SetTrigger("MeleeAttack");
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

    void LaunchProjectile()
    {
        Instantiate(projectilePrefab, attackPoint.position, Quaternion.LookRotation(player.position - attackPoint.position));
    }

    public void MeleeAttackDamage()
    {
        
    }
}
