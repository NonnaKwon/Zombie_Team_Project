using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitManager : MonoBehaviour
{
    public static TakeHitManager Instance;

    public GameObject bloodEffectPrefab;
    private Queue<GameObject> bloodEffectPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of TakeHitManager found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize(10);
    }

    void Initialize(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject instance = Instantiate(bloodEffectPrefab);
            instance.SetActive(false);
            bloodEffectPool.Enqueue(instance);
            instance.transform.SetParent(transform);
        }
    }

    // ���� ȿ�� ��ü�� Ǯ���� �������� �޼���
    public GameObject GetBloodEffect()
    {
        if (bloodEffectPool.Count > 0)
        {
            GameObject bloodEffect = bloodEffectPool.Dequeue();
            bloodEffect.SetActive(true);
            return bloodEffect;
        }
        else
        {
            // Ǯ�� ��������� ���ο� ���� ȿ�� ��ü�� ����
            GameObject bloodEffect = Instantiate(bloodEffectPrefab);
            bloodEffect.SetActive(true);
            return bloodEffect;
        }
    }

    // ����� ���� ȿ�� ��ü�� �ٽ� Ǯ�� ��ȯ�ϴ� �޼���
    public void ReturnToPool(GameObject bloodEffect)
    {
        bloodEffect.SetActive(false);
        bloodEffectPool.Enqueue(bloodEffect);
    }
}
