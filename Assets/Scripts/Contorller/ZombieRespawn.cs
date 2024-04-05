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
    public float checkRadius; // ������ �ݰ� üũ

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

                Transform spawnPoint = FindSpawnLocation();
                if (spawnPoint != null)
                {
                    int prefabIndex = Random.Range(0, zombiePrefabs.Count);
                    Instantiate(zombiePrefabs[prefabIndex], spawnPoint.position, Quaternion.identity);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    Transform FindSpawnLocation()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (!Physics.CheckSphere(spawnPoint.position, checkRadius, LayerMask.GetMask("Zombie")))
            {
                validSpawnPoints.Add(spawnPoint); //
            }
        }

        if (validSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);
            return validSpawnPoints[randomIndex];
        }
        else
        {
            return null; // ������ ���� ��ġ�� ã�� ���� ���
        }
    }


}