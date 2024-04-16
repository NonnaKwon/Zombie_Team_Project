using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ZombieRespawn : MonoBehaviour
{
    public Transform[] spawnPoints; // ���� ������ ��ġ
    public List<PooledObject> zombiePrefabs; // ���� ������ ����Ʈ
    public LayerMask layer;

    public float spawnTime; // ���� ������ �ð� ����
    public bool isGameOver = false;
    public float checkRadius; // ������ �ݰ� üũ

    private void Start()
    {
        zombiePrefabs.Add(Manager.Resource.Load<PooledObject>("Prefabs/RunZombie"));
        zombiePrefabs.Add(Manager.Resource.Load<PooledObject>("Prefabs/WalkZombie"));
        zombiePrefabs.Add(Manager.Resource.Load<PooledObject>("Prefabs/CrawlZombie"));
        if (spawnPoints.Length > 0)
        {
            StartCoroutine(SpawnZombie());
        }
    }

    IEnumerator SpawnZombie()
    {
        if(!Manager.Game.IsSpawn)
            yield return new WaitForSeconds(SPAWN_TIME); //3��

        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            Vector3 spawnPoint = FindSpawnLocation();
            if (Physics.Raycast(Camera.main.transform.position, (spawnPoint - Camera.main.transform.position).normalized, out RaycastHit hit))
            {
                if (layer.Contain(hit.collider.gameObject.layer))
                {
                    int prefabIndex = Random.Range(0, zombiePrefabs.Count);
                    Manager.Pool.GetPool(zombiePrefabs[prefabIndex], spawnPoint, Quaternion.identity);
                }
            }
        }
    }


    Vector3 FindSpawnLocation()
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        float randomX = Random.Range(-checkRadius, checkRadius);
        float randomZ = Random.Range(-checkRadius, checkRadius);
        return new Vector3(spawnPoints[randomPoint].position.x + randomX, 0, spawnPoints[randomPoint].position.z + randomZ);
    }
}