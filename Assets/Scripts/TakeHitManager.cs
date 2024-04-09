using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitManager : MonoBehaviour
{
    public static TakeHitManager Instance;

    public GameObject bloodEffectPrefab; // ���� ȿ�� ������
    private Queue<GameObject> bloodEffectPool = new Queue<GameObject>(); // ���� ȿ���� ������ ť

    void Awake()
    {
        Instance = this;
        Initialize(10); // ���÷� 10���� ���� ȿ���� �ʱ�ȭ
    }

    void Initialize(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            bloodEffectPool.Enqueue(CreateNewBloodEffect());
        }
    }

    GameObject CreateNewBloodEffect()
    {
        var obj = Instantiate(bloodEffectPrefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    public static GameObject GetBloodEffect()
    {
        if (Instance.bloodEffectPool.Count > 0)
        {
            var obj = Instance.bloodEffectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instance.CreateNewBloodEffect();
        }
    }

    public static void ReturnBloodEffect(GameObject bloodEffect)
    {
        bloodEffect.SetActive(false);
        Instance.bloodEffectPool.Enqueue(bloodEffect);
    }
}
