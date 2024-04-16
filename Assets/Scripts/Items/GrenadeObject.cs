using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    [SerializeField] LayerMask _layer;
    WeaponData _data;
    Rigidbody _rigid;
    float _power;
    AttackPoint _attackPoint;
    PooledObject _explosion;
    AudioClip _audio;

    private void Awake()
    {
        _audio = Manager.Resource.Load<AudioClip>("Sounds/Grenade");

        _rigid = GetComponent<Rigidbody>();
        _attackPoint = GetComponentInChildren<AttackPoint>();
        _data = Manager.Resource.Load<WeaponData>("Data/Grenade");
        _explosion = Manager.Resource.Load<PooledObject>("Prefabs/Effects/Explosion");
        _power = 4f;
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
        StartCoroutine(CoForce(targetPos));
    }

    IEnumerator CoForce(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        Vector3 mid = new Vector3(startPos.x + (targetPos.x - startPos.x) / 2, 10f, startPos.z + (targetPos.z - startPos.z) / 2);
        float time = 0;
        float duration = 1.2f;
        while (true)
        {
            if (time >= duration)
                break;

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
        Manager.Sound.PlaySFX(_audio);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}
