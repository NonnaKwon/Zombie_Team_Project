using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class BossZombie : MonoBehaviour, IDamagable
{
    public int maxHp = 100;
    private int curHp;
    public float attackRange = 1.5f;
    public float sightRange = 15.0f; // 플레이어 감지 범위
    public float zombieSpeed;
    public Transform attackPoint; // 공격 발사 위치
    public GameObject zombiePrefab; // 소환할 좀비 프리팹
    public Transform[] CreatePoints; // 좀비 소환 위치
    private Transform player;
    private Animator animator;
    private Rigidbody rigid;
    public GameObject[] dropItem;
    [SerializeField] ZombieType type;
    [SerializeField] float attackDamage;
    private PooledObject bloodEffect;
    private PooledObject fireBloodEffect;
    public Transform FireBloodPoint;


    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;
    private ZombieState currentState;
    private enum ZombieState
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    enum ZombieType
    {
        Boss
    }
    void Start()
    {
        currentPhase = BossPhase.Phase3;

        curHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(PhaseManager());
        bloodEffect = Manager.Resource.Load<PooledObject>("Prefabs/Effects/BloodEffect");
        fireBloodEffect = Manager.Resource.Load<PooledObject>("Prefabs/Effects/FireBloodEffect");
    }
    private void Awake()
    {
        player = Manager.Game.Player.gameObject.transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
        animator.SetInteger("ZombieType", (int)type);
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
                CancelInvoke("CreateZombies"); // 3페이즈가 되면 소환 중단
            }
            else if (curHp < maxHp * 0.7 && currentPhase == BossPhase.Phase1)
            {
                currentPhase = BossPhase.Phase2;
                CreateZombie();
            }

            yield return new WaitForSeconds(5f); // 5초마다 체크
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
        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
        }

        switch (currentPhase)
        {
            case BossPhase.Phase1:
                // 1페이즈: 플레이어가 근접 공격 범위 내에 있을 경우 근접 공격 실행
                if (distanceToPlayer <= attackRange)
                {
                    animator.Play("Attack");
                }
                break;
            case BossPhase.Phase2:
                Debug.Log("2페이즈");
                CreateZombie();
                break;
            case BossPhase.Phase3:
                // 3페이즈: 원거리 공격 실행. 플레이어가 더 멀리 있을 경우 원거리 공격을 사용
                Debug.Log("3페이즈");
                if (!IsInvoking("FireBlood"))
                {
                    InvokeRepeating("FireBlood", 0f, 2f); // 2초마다 피토 발사
                }
                break;
        }

        if (fireBloodEffect != null && FireBloodPoint != null)
        {
            // FireBlood 이펙트를 FireBloodPoint 위치에 생성
            var effect = PoolManager.Instance.GetPool(fireBloodEffect, FireBloodPoint.position, Quaternion.identity);
            effect.transform.forward = FireBloodPoint.forward; // 이펙트 방향 조정
        }
    }

    void StartCreateZombies()
    {
        StartCoroutine(CreateZombiesRoutine());
    }

    IEnumerator CreateZombiesRoutine()
    {
        int count = 0;
        while (count < 200) // 총 200마리 생성
        {
            CreateZombie(); // 단일 좀비 생성
            count++;
            if (count % 10 == 0) // 10마리마다 잠시 대기
                yield return new WaitForSeconds(2);
        }
    }

    void CreateZombie()
    {
        Transform CreatePoint = CreatePoints[Random.Range(0, CreatePoints.Length)];
        Instantiate(zombiePrefab, CreatePoint.position, Quaternion.identity);
    }

    void HealBoss()
    {
        if (currentPhase == BossPhase.Phase2)
        {
            curHp += 1000; // 예시: 체력 1000 회복
            curHp = Mathf.Min(curHp, maxHp); // 최대 체력을 초과하지 않게 조정
        }
    }
    void FireBlood()
    {
        animator.Play("FireBlood");
        Manager.Pool.GetPool(fireBloodEffect, transform.position + new Vector3(1f, 3f, 0), transform.rotation);

        if (Vector3.Distance(transform.position, player.transform.position) < 3f) // 플레이어가 좀비로부터 n 미터 이내에 있을 경우
        {
            // player.TakeDamage(10); // 플레이어 TakeDamage 설정
        }
    }


    public void TakeDamage(float damage)
    {
        curHp -= (int)damage;

        Manager.Pool.GetPool(bloodEffect, transform.position + new Vector3(0, 2.5f, 0), transform.rotation);

        // 혈흔 효과 생성
        //GameObject bloodEffect = TakeHitManager.Instance.GetBloodEffect();
        //bloodEffect.transform.position = transform.position + new Vector3(0,2f,0); // 혈흔 효과 위치를 좀비 위치로 설정

        //StartCoroutine(ReturnBloodEffectToPool(bloodEffect));

        if (curHp <= 0)
        {
            Die();
            Debug.Log("보스 죽음");
        }
    }

    IEnumerator ReturnBloodEffectToPool(GameObject bloodEffect)
    {
        yield return new WaitForSeconds(1); // 혈흔 효과 지속 시간
        //TakeHitManager.Instance.ReturnToPool(bloodEffect);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bat"))
        {
            SoundManager.instance.PlayMeleeHitSound();
        }
    }

    private void Die()
    {
        animator.Play("Die");
        Manager.Game.BossCount += 1;
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
