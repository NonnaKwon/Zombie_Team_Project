using UnityEngine;

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
        bloodEffect = Resources.Load<GameObject>("Weapon");
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
            Debug.Log("플레이어 공격");
            attackPoint.Hit(attackDamage);
            animator.SetBool("IsAttack", true);
            animator.SetBool("Bite", true);
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
        animator.SetTrigger("TakeHit");
        // Manager. 오브젝트풀 사용
        if (hp <= 0)
        {
            Die();
            Debug.Log("좀비 죽음");
        }
    }
}
