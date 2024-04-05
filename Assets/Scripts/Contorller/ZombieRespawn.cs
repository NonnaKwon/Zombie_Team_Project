using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRespawn : MonoBehaviour
{
    public Transform[] spawnPoints; // 좀비를 생성할 위치
    public List<GameObject> zombiePrefabs; // 좀비 프리팹 리스트

    public float spawnTime; // 좀비를 생성할 시간 간격
    public int maxZombies; // 최대 좀비 수
    public bool isGameOver = false;
    public float checkRadius; // 리스폰 반경 체크

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
            return null; // 적합한 스폰 위치를 찾지 못한 경우
        }
    }


}