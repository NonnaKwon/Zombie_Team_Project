using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    MapController _mapPrefab;
    private bool _spawnComplete = false;
    private int[] _rotateRandomList = { 0, 90, 180 };

    private const float POS_DISTANCE = 70f;

    void Start()
    {
        _mapPrefab = Manager.Resource.Load<MapController>("Prefabs/Map");
    }

    public void SpawnMap()
    {
        Spawn(new Vector3(-POS_DISTANCE, 0, 0)); //왼
        Spawn(new Vector3(POS_DISTANCE, 0, 0)); //오
        Spawn(new Vector3(0, 0, -POS_DISTANCE)); //아래
        Spawn(new Vector3(0, 0, POS_DISTANCE)); //위

        Spawn(new Vector3(-POS_DISTANCE, 0, POS_DISTANCE)); //대각선 왼-위
        Spawn(new Vector3(-POS_DISTANCE, 0, -POS_DISTANCE)); //대각선 왼-아래
        Spawn(new Vector3(POS_DISTANCE, 0, POS_DISTANCE)); //대각선 오른-위
        Spawn(new Vector3(POS_DISTANCE, 0, -POS_DISTANCE)); //대각선 오른-아래

        _spawnComplete = true;
    }

    private void Spawn(Vector3 plusPos)
    {
        if (_spawnComplete)
            return;

        //맵이 없을때 스폰해야함. 있는지 확인
        //해당 방향에 맵이 있는지 없는지는, 카메라 - 해당 위치 레이를 쏴서 구현하자.
        Vector3 spawnPos = transform.position + plusPos;
        Debug.Log(spawnPos.ToString());
        Vector3 startPos = Camera.main.transform.position + new Vector3(0,100f,0);
        Debug.DrawRay(startPos, (spawnPos - startPos),Color.red,100f);
        if (Physics.Raycast(startPos, (spawnPos - startPos),out RaycastHit hit))
        {
            //뭔가에 부딫혔다
            if (hit.collider.gameObject.GetComponent<MapController>() != null)
            {
                Debug.Log("맵이 있음");
                return;
            }
        }

        //맵이 없다면, 스폰 시작
        int randomRotate = Random.Range(0, 3);

        MapController spawnMap = Instantiate<MapController>(_mapPrefab, spawnPos, transform.rotation);
        spawnMap.transform.Rotate(new Vector3(0, _rotateRandomList[randomRotate], 0));
        spawnMap.transform.parent = transform;
    }
}
