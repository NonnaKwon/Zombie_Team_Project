using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    [SerializeField] LayerMask _layer;
    WeaponData _data;
    Rigidbody _rigid;
    float _power;
    AttackPoint _attackPoint;
    PooledObject _explosion;
    AudioClip explosionSound;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _attackPoint = GetComponentInChildren<AttackPoint>();
        _data = Manager.Resource.Load<WeaponData>("Data/Grenade");
        _explosion = Manager.Resource.Load<PooledObject>("Prefabs/Effects/Explosion");
        _power = 4f;
        explosionSound = Resources.Load<AudioClip>("Grenade");
    }
    private void OnTriggerEnter(Collider other)
    {
        MapController map = other.GetComponent<MapController>();
        if (map != null)
            StartCoroutine(Bang());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_layer.Contain(collision.gameObject.layer))
            StartCoroutine(Bang());
    }

    public void ForwardForce(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        _rigid.velocity = new Vector3(0, _power, 0) + dir * _power;
    }

    public void ForwardForce2(Vector3 targetPos)
    {
        StartCoroutine(CoForce(targetPos));
    }

    IEnumerator CoForce(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        Vector3 mid = new Vector3(startPos.x + (startPos.x - targetPos.x) / 2, 10f, startPos.z + (startPos.z - targetPos.z) / 2);
        Debug.Log(startPos + ":" + targetPos + "=" + mid);
        float time = 0;
        float duration = 2f;
        while (true)
        {
            if (time > 1f)
                time = 0f;

            Vector3 p1 = Vector3.Lerp(startPos, mid, time);
            Vector3 p2 = Vector3.Lerp(mid, targetPos, time);
            transform.position = Vector3.Lerp(p1, p2, time);

            time += Time.deltaTime / duration;
            yield return null;
        }
    }


    IEnumerator Bang()
    {
        _attackPoint.SetRange(_data.attackRange);
        _attackPoint.Hit(_data.minDamage, _data.maxDamage);
        Manager.Pool.GetPool(_explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        SoundManager.Instance.PlaySFX(explosionSound);
    }
}
