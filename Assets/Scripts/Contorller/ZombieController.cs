using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieController : MonoBehaviour, IDamagable
{
    [SerializeField] ZombieType type;
    [SerializeField] AttackPoint attackPoint;
    [SerializeField] float attackDamage;
    public Transform player;
    public Rigidbody rigid;
    public Animator animator;

    public float sightRange, attackRange;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float zombieSpeed;
    private float time = 0;
    public float MoveSpeed { get { return zombieSpeed; } }
    private Vector3 moveDir;
    private GameObject bloodEffect;

    private ZombieState currentState;
    public float hp = 100;
    public GameObject[] dropItems;
    public GameObject bloodEffectPrefab;

    private enum ZombieState
    {
        Idle,
        Chase,
        Attack
    }

    enum ZombieType
    {
        walk,
        run,
        crawl
    }
    private void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Blood");
    }

    private void Awake()
    {
        player = Manager.Game.Player.gameObject.transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
        animator.SetInteger("ZombieType", (int)type);
        attackPoint = GetComponentInChildren<AttackPoint>();
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
        // MoveAnimator();
    }

    //private void MoveAnimator()
    //{
    //    animator.SetFloat("velocity", (moveDir * zombieSpeed).magnitude);
    //}

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

    private void AttackPlayer()
    {
        time += Time.deltaTime;
        if (time > timeBetweenAttacks)
        {
            time = 0;
            Debug.Log("�÷��̾� ����");
            attackPoint.Hit(attackDamage);
            if (ZombieType.crawl == type)
                animator.Play("Bite");
            else
                animator.Play("Attack");
        }

        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
        }
    }


    private void Die()
    {
        animator.SetTrigger("Die");
        DropItem();
        Destroy(gameObject);
        if(hp <= 0)
        {
            Destroy(bloodEffect);
        }
    }

    private void DropItem()
    {
        if (dropItems.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        // ���� ȿ�� ����
        GameObject bloodEffect = TakeHitManager.Instance.GetBloodEffect();
        bloodEffect.transform.position = transform.position; // ���� ȿ�� ��ġ�� ���� ��ġ�� ����

        StartCoroutine(ReturnBloodEffectToPool(bloodEffect));

        if (hp <= 0)
        {
            Die();
            Debug.Log("���� ����");
        }
    }

    IEnumerator ReturnBloodEffectToPool(GameObject bloodEffect)
    {
        yield return new WaitForSeconds(1); // ���� ȿ�� ���� �ð�
        TakeHitManager.Instance.ReturnToPool(bloodEffect);
    }
}
