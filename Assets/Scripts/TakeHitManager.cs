using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitManager : MonoBehaviour
{
    public static TakeHitManager Instance;

    public GameObject bloodEffectPrefab; // Ç÷Èç È¿°ú ÇÁ¸®ÆÕ
    private Queue<GameObject> bloodEffectPool = new Queue<GameObject>(); // Ç÷Èç È¿°ú¸¦ ÀúÀåÇÒ Å¥

    void Awake()
    {
        Instance = this;
        Initialize(10); // ¿¹½Ã·Î 10°³ÀÇ Ç÷Èç È¿°ú¸¦ ÃÊ±âÈ­
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
