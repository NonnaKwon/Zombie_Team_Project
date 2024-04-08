using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRespawn : MonoBehaviour
{
    public Transform[] spawnPoints; // ���� ������ ��ġ
    public List<GameObject> zombiePrefabs; // ���� ������ ����Ʈ

    public float spawnTime; // ���� ������ �ð� ����
    public int maxZombies; // �ִ� ���� ��
    public bool isGameOver = false;

    private void Start()
    {
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(SpawnZombie());
        }
    }

    IEnumerator SpawnZombie()
    {
        while (!isGameOver)
        {
            int zombieCount = GameObject.FindGameObjectsWithTag("Zombie").Length;

            if (zombieCount < maxZombies)
            {
                yield return new WaitForSeconds(spawnTime);

                int spawnIndex = Random.Range(0, spawnPoints.Length); // ���� ��ġ ����
                int prefabIndex = Random.Range(0, zombiePrefabs.Count); // ������ ����

                Instantiate(zombiePrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
            }
            else
            {
                yield return null;
            }
        }
    }
}