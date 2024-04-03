using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] Transform _muzzlePoint;
    private float _maxDistance = 10f;
    LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    public override void Attack()
    {
        float damage = Random.Range(_data.minDamage, _data.maxDamage);
        Vector3 dir = Manager.Game.Player.transform.forward;
        Debug.Log(dir);
        RaycastHit hitInfo;
        //muzzleFlash.Play(); -> ���η�������
        if (Physics.Raycast(_muzzlePoint.position, dir, out hitInfo, _maxDistance))
        {
            IDamagable damagable = hitInfo.collider.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage);

            //ParticleSystem effect = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //effect.transform.parent = hitInfo.transform;

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