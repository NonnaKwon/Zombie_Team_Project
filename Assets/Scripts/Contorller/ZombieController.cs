
using UnityEngine;

public class ZombieController : MonoBehaviour, IDamagable
{
    [SerializeField] ZombieType type;
    public Transform player;
    public Rigidbody rigid;
    public Animator animator;

    public float sightRange, attackRange;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float zombieSpeed;

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

    private void Awake()
    {
        player = Manager.Game.Player.gameObject.transform;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = ZombieState.Idle;
        animator.SetInteger("ZombieType", (int)type);
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

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            Debug.Log("플레이어 공격");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // overlap 사용해서 공격 구현

        }

        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            currentState = ZombieState.Chase;
            animator.SetBool("IsChase", true);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
   

    private void Die()
    {
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
        animator.SetTrigger("Hit");
        if (hp <= 0)
        {
            Die();
        }
    }
}
