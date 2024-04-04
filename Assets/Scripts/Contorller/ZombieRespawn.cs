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

                int spawnIndex = Random.Range(0, spawnPoints.Length); // 스폰 위치 선택
                int prefabIndex = Random.Range(0, zombiePrefabs.Count); // 프리팹 선택

                Instantiate(zombiePrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
            }
            else
            {
                yield return null;
            }
        }
    }
}