using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    MapController _mapPrefab;
    private bool _spawnComplete = false;
    private int[] _rotateRandomList = { 0, 90, 180 };
    private const float POS_DISTANCE = 68f;
    private float DESTROY_DISTANCE = POS_DISTANCE * 3;
    PlayerController _player;

    void Start()
    {
        _mapPrefab = Manager.Resource.Load<MapController>("Prefabs/Map");
        _player = Manager.Game.Player;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.gameObject.transform.position) >= DESTROY_DISTANCE)
            Destroy(gameObject);
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
        Vector3 startPos = Camera.main.transform.position + new Vector3(0,100f,0);
        Debug.DrawRay(startPos, (spawnPos - startPos),Color.red,3);
        if (Physics.Raycast(startPos, (spawnPos - startPos),out RaycastHit hit))
        {
            //뭔가에 부딫혔다
            if (hit.collider.gameObject.GetComponent<MapController>() != null
                || hit.collider.gameObject.GetComponentInParent<MapController>() != null)
            {
                return;
            }
        }

        //맵이 없다면, 스폰 시작
        int randomRotate = Random.Range(0, 3);

        MapController spawnMap = Instantiate(_mapPrefab, spawnPos, transform.rotation);
        spawnMap.transform.Rotate(new Vector3(0, _rotateRandomList[randomRotate], 0));
        spawnMap.transform.parent = transform.root;
    }
}
