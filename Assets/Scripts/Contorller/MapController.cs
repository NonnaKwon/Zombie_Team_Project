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
        Spawn(new Vector3(-POS_DISTANCE, 0, 0)); //��
        Spawn(new Vector3(POS_DISTANCE, 0, 0)); //��
        Spawn(new Vector3(0, 0, -POS_DISTANCE)); //�Ʒ�
        Spawn(new Vector3(0, 0, POS_DISTANCE)); //��

        Spawn(new Vector3(-POS_DISTANCE, 0, POS_DISTANCE)); //�밢�� ��-��
        Spawn(new Vector3(-POS_DISTANCE, 0, -POS_DISTANCE)); //�밢�� ��-�Ʒ�
        Spawn(new Vector3(POS_DISTANCE, 0, POS_DISTANCE)); //�밢�� ����-��
        Spawn(new Vector3(POS_DISTANCE, 0, -POS_DISTANCE)); //�밢�� ����-�Ʒ�

        _spawnComplete = true;
    }

    private void Spawn(Vector3 plusPos)
    {
        if (_spawnComplete)
            return;

        //���� ������ �����ؾ���. �ִ��� Ȯ��
        //�ش� ���⿡ ���� �ִ��� ��������, ī�޶� - �ش� ��ġ ���̸� ���� ��������.
        Vector3 spawnPos = transform.position + plusPos;
        Debug.Log(spawnPos.ToString());
        Vector3 startPos = Camera.main.transform.position + new Vector3(0,100f,0);
        Debug.DrawRay(startPos, (spawnPos - startPos),Color.red,100f);
        if (Physics.Raycast(startPos, (spawnPos - startPos),out RaycastHit hit))
        {
            //������ �΋H����
            if (hit.collider.gameObject.GetComponent<MapController>() != null)
            {
                Debug.Log("���� ����");
                return;
            }
        }

        //���� ���ٸ�, ���� ����
        int randomRotate = Random.Range(0, 3);

        MapController spawnMap = Instantiate<MapController>(_mapPrefab, spawnPos, transform.rotation);
        spawnMap.transform.Rotate(new Vector3(0, _rotateRandomList[randomRotate], 0));
        spawnMap.transform.parent = transform;
    }
}
