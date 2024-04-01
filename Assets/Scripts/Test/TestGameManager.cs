using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject ZombiePrefab;

    public float createTime;
    public int maxZombie;
    public bool isGameOver = false;

    private void Start()
    {
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if(points.Length > 0)
        {
            StartCoroutine(this.CreateZombie());
        }
    }

    IEnumerator CreateZombie()
    {
        while (!isGameOver)
        {
            int zombieCount = (int)GameObject.FindGameObjectsWithTag("Zombie").Length;

            if (zombieCount < maxZombie)
            {
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length);
                
                Instantiate(ZombiePrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}
