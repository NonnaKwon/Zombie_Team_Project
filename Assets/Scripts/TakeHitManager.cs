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

    // 혈흔 효과 객체를 풀에서 가져오는 메서드
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
            // 풀이 비어있으면 새로운 혈흔 효과 객체를 생성
            GameObject bloodEffect = Instantiate(bloodEffectPrefab);
            bloodEffect.SetActive(true);
            return bloodEffect;
        }
    }

    // 사용한 혈흔 효과 객체를 다시 풀로 반환하는 메서드
    public void ReturnToPool(GameObject bloodEffect)
    {
        bloodEffect.SetActive(false);
        bloodEffectPool.Enqueue(bloodEffect);
    }
}
