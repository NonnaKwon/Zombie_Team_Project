using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombie : MonoBehaviour
{
    public int maxHp = 5000;
    private int curHp;
    public float meleeAttackRange = 1.5f;
    public float noticeRange = 15.0f; // 플레이어 감지 범위
    public GameObject projectilePrefab; // 원거리 공격용 프로젝트
    public Transform attackPoint; // 공격 발사 위치
    public GameObject zombiePrefab; // 소환할 좀비 프리팹
    public Transform[] CreatePoints; // 좀비 소환 위치
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
                CancelInvoke("CreateZombies"); // 3페이즈가 되면 소환 중단
            }
            else if (curHp < maxHp * 0.7 && currentPhase == BossPhase.Phase1)
            {
                currentPhase = BossPhase.Phase2;
                CreateZombies(); // 2페이즈에서 한 번에 좀비 200마리 소환
            }

            yield return new WaitForSeconds(5f); // 5초마다 체크
        }
    }

    void AttackPlayer(float distanceToPlayer)
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                // 1페이즈: 플레이어가 근접 공격 범위 내에 있을 경우 근접 공격 실행
                if (distanceToPlayer <= meleeAttackRange)
                {
                    animator.SetTrigger("MeleeAttack");
                }
                break;
            case BossPhase.Phase2:
                break;
            case BossPhase.Phase3:
                // 3페이즈: 원거리 공격 실행. 플레이어가 더 멀리 있을 경우 원거리 공격을 사용
                if (!IsInvoking("FireBlood"))
                {
                    InvokeRepeating("FireBlood", 0f, 2f); // 2초마다 피토 발사
                }
                break;
        }
    }

    void CreateZombies()
    {
        for (int i = 0; i < 200; i++) // 200마리 소환
        {
            // 소환 위치를 랜덤하게 결정하기 위해 CreatePoints 중 하나를 무작위로 선택
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
