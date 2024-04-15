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
    public AttackPoint attackPoint; // 공격 발사 위치
    public GameObject zombiePrefab; // 소환할 좀비 프리팹
    private Transform player;
    private Animator animator;
    private Rigidbody rigid;
    public GameObject[] dropItem;
    [SerializeField] ZombieType type;
    [SerializeField] float attackDamage;
    private PooledObject bloodEffect;
    private PooledObject fireBloodEffect;
    public Transform FireBloodPoint;


    private float time = 0;
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
            case ZombieState.Attack:
                AttackPlayer();
                break;
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

    void AttackPlayer()
    {
        time += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer > attackRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
            return;
        }

        if (time <= 2f) //2초마다 공격
            return;
        time = 0;
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                StartCoroutine(Attack());
                break;
            case BossPhase.Phase2:
                Debug.Log("2페이즈");
                StartCoroutine(Attack());
                CreateZombie();
                break;
            case BossPhase.Phase3:
                StartCoroutine(Attack());
                break;
        }
    }

    IEnumerator Attack()
    {
        animator.Play("Attack");
        yield return new WaitForSeconds(1.4f);
        attackPoint.Hit(attackDamage);
        if (currentPhase == BossPhase.Phase3)
        {
            yield return new WaitForSeconds(0.6f);
            animator.Play("FireBlood");
            yield return new WaitForSeconds(0.6f);

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
        float range = 10f;
        Vector3 InstPos = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        Instantiate(zombiePrefab, InstPos, Quaternion.identity);
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
    }


    public void TakeDamage(float damage)
    {
        curHp -= (int)damage;

        Manager.Pool.GetPool(bloodEffect, transform.position + new Vector3(0, 2.5f, 0), transform.rotation);

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
