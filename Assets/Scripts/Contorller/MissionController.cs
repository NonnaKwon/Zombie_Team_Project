using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    [SerializeField] List<Mission> _missionList = new List<Mission>();

    private int _nowMission = 0; //index ���õ����ͷ� ������ (�������ߴ���)

    public UI_Mission ConnectUI { set { _connectUI = value; } }
    UI_Mission _connectUI;


    private void Awake()
    {
        Manager.Game.Mission = this;
        _connectUI = Manager.Game.GameUI.GetComponentInChildren<UI_Mission>();
        _connectUI.SetCurrentMission(_missionList[_nowMission]);
    }

    private void Start()
    {

    }

    public Mission CurrentMission()
    {
        return _missionList[_nowMission];
    }

    public void MissionComplete()
    {
        Debug.Log("�̼ǿϷ�");
        _missionList[_nowMission].isComplete = true;
    }
}
