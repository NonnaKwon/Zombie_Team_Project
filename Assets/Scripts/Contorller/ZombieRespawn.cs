using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRespawn : MonoBehaviour
{
    public Transform[] spawnPoints; // ���� ������ ��ġ
    public List<GameObject> zombiePrefabs; // ���� ������ ����Ʈ
    public LayerMask layer;

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

                Vector3 spawnPoint = FindSpawnLocation(); // 
                if (spawnPoint != null)
                {
                    if(Physics.Raycast(Camera.main.transform.position,(spawnPoint - Camera.main.transform.position).normalized ,out RaycastHit hit))
                    {
                        Debug.DrawRay(Camera.main.transform.position, (spawnPoint - Camera.main.transform.position).normalized, Color.blue,hit.distance*100f);
                        Debug.Log(layer.Contain(hit.collider.gameObject.layer));

                        if (layer.Contain(hit.collider.gameObject.layer))
                        {
                            int prefabIndex = Random.Range(0, zombiePrefabs.Count);
                            Instantiate(zombiePrefabs[prefabIndex], spawnPoint, Quaternion.identity);
                        }
                    }
                }
            }
            else
            {
                yield return null;
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

    // ����ĳ��Ʈ�� �����ؼ� �ǹ� ������ ���� �������� �ʰ� ����!

    //Transform FindSpawnLocation()
    //{
    //    List<Transform> validSpawnPoints = new List<Transform>();

    //    foreach (var spawnPoint in spawnPoints)
    //    {
    //        if (!Physics.CheckSphere(spawnPoint.position, checkRadius, LayerMask.GetMask("Zombie")))
    //        {
    //            validSpawnPoints.Add(spawnPoint); //
    //        }
    //    }

    //    if (validSpawnPoints.Count > 0)
    //    {
    //        int randomIndex = Random.Range(0, validSpawnPoints.Count);
    //        return validSpawnPoints[randomIndex];
    //    }
    //    else
    //    {
    //        return null; // ������ ���� ��ġ�� ã�� ���� ���
    //    }
    //}


}