using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : Weapon
{
    [SerializeField] Transform _muzzlePoint;
    [SerializeField] int _maxBullet;

    private float _maxDistance = 10f;
    private int _curBullet;

    LineRenderer _lineRenderer;
    RigBuilder _rigBuilder;

    protected override void Awake()
    {
        base.Awake();
        _curBullet = _maxBullet;
        _lineRenderer = GetComponent<LineRenderer>();
        _rigBuilder = GetComponentInParent<RigBuilder>();
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
        if (_curBullet == 0)
            return;
        _curBullet--;
        Debug.Log(_curBullet);

        float damage = Random.Range(_data.minDamage, _data.maxDamage);
        Vector3 dir = Manager.Game.Player.transform.forward;

        if (Physics.Raycast(_muzzlePoint.position, dir, out RaycastHit hitInfo, _maxDistance))
        {
            IDamagable damagable = hitInfo.collider.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage);

            Rigidbody rigid = hitInfo.collider.GetComponent<Rigidbody>();
            if (rigid != null)
                rigid.AddForceAtPosition(-hitInfo.normal * 10f, hitInfo.point, ForceMode.Impulse); //레이캐스트 닿는곳에 Addforce할 수 있도록 position에 힘을 가해라! 하는 함수임
        }
        else
        {
            Debug.Log("총을 쐇을 때 아무것도 안맞았다.");
        }

        StartCoroutine(DrawLine(_muzzlePoint.position, dir));
        _playerAnimator.Play("fire");
    }

    IEnumerator DrawLine(Vector3 startPos,Vector3 dir)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, startPos + dir * 100f);
        yield return new WaitForSeconds(0.1f);
        _lineRenderer.positionCount = 0;
    }
}