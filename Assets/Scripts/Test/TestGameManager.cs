using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public Transform[] spawnPoints; // 좀비를 생성할 위치
    public List<GameObject> zombiePrefabs; // 좀비 프리팹 리스트
    public LayerMask whatIsZombie;

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
            if (GameObject.FindGameObjectsWithTag("Zombie").Length < maxZombies)
            {
                yield return new WaitForSeconds(spawnTime);

                Transform spawnPoint = ChooseSpawnPoint();
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


    Transform ChooseSpawnPoint()
    {
        List<Transform> validSpawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Physics.OverlapSphere(spawnPoint.position, 1f, whatIsZombie).Length == 0)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        if (validSpawnPoints.Count > 0)
        {
            return validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        }
        else
        {
            return null;
        }
    }
}
