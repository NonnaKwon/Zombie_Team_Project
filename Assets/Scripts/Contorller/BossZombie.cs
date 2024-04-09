using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombie : MonoBehaviour, IDamagable
{
    [SerializeField] AttackPoint attackPoint;
    [SerializeField] float attackDamage;
    public GameObject[] dropItem;
    public float attackRange;
    public float speed;
    public int maxHp = 5000;
    public int curHp;

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase curPhase = BossPhase.Phase1;

    void Start()
    {
        curHp = maxHp;
    }

    void Update()
    {
        CheckPhase();
    }

    private void CheckPhase()
    {
        float hpPercentage = (float)curHp / maxHp;

        if (hpPercentage > 0.7f)
        {
            SetPhase(BossPhase.Phase1);
        }
        else if (hpPercentage > 0.3f)
        {
            SetPhase(BossPhase.Phase2);
        }
        else
        {
            SetPhase(BossPhase.Phase3);
        }
    }

    private void SetPhase(BossPhase newPhase)
    {
        if (curPhase != newPhase)
        {
            curPhase = newPhase;
            OnPhaseChange(newPhase);
        }
    }

    private void OnPhaseChange(BossPhase phase)
    {
        switch (phase)
        {
            case BossPhase.Phase1:
                // ���� ����
                break;
            case BossPhase.Phase2:
                // ���� 200���� ��ȯ �� ü�� ȸ��
                break;
            case BossPhase.Phase3:
                // ���Ÿ� ����
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        curHp -= (int)damage;
        if (curHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("���� ���");
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
