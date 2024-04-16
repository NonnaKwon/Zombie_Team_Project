using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : Weapon
{
    [SerializeField] Transform _muzzlePoint;
    [SerializeField] int _maxBullet;

    private int _curBullet;

    LineRenderer _lineRenderer;
    RigBuilder _rigBuilder;
    Inventory _inventory;
    PooledObject _muzzleFlash;

    protected override void Awake()
    {
        base.Awake();
        _curBullet = _maxBullet;
        _lineRenderer = GetComponent<LineRenderer>();
        _rigBuilder = GetComponentInParent<RigBuilder>();
        _muzzleFlash = Manager.Resource.Load<PooledObject>("Prefabs/Effects/MuzzleFlash");
    }

    private void Start()
    {
        _inventory = Manager.Game.Player.GetComponent<Inventory>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rigBuilder.layers[0].active = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _rigBuilder.layers[0].active = false;
    }
   
    public override void Attack()
    {
        if (_inventory.BulletCount == 0)
            return;
        OnAttack = true;
        _inventory.BulletCount--;
        Manager.Pool.GetPool(_muzzleFlash, _muzzlePoint.position, _muzzlePoint.rotation);
        Vector3 dir = Manager.Game.Player.transform.forward;
        if (Physics.Raycast(_muzzlePoint.position, dir, out RaycastHit hitInfo, _data.attackRange))
        {
            IDamagable damagable = hitInfo.collider.GetComponent<IDamagable>();

            if (damagable != null)
            {
                //���� ����
                float damage = Random.Range(_data.minDamage, _data.maxDamage);
                damagable.TakeDamage(damage);
                Debug.Log("Zombie : ���ݴ���");
            }

            Rigidbody rigid = hitInfo.collider.GetComponent<Rigidbody>();
            if (rigid != null)
                rigid.AddForceAtPosition(-hitInfo.normal * 10f, hitInfo.point, ForceMode.Impulse); //����ĳ��Ʈ ��°��� Addforce�� �� �ֵ��� position�� ���� ���ض�! �ϴ� �Լ���
        }
        else
        {
            Debug.Log("���� �i�� �� �ƹ��͵� �ȸ¾Ҵ�.");
        }

        StartCoroutine(DrawLine(_muzzlePoint.position, dir));
        _playerAnimator.Play("fire");
        _playerAnimator.SetFloat("attackSpeed", AttackSpeed);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.gunshotSound);
    }

    IEnumerator DrawLine(Vector3 startPos,Vector3 dir)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, startPos + dir * 100f);
        yield return new WaitForSeconds(0.1f / AttackSpeed);
        _lineRenderer.positionCount = 0;
        OnAttack = false;
    }

}
