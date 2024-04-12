using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    private int _nowMission;
    private int _coinGoal;
    private int _bossGoal;

    UI_Mission _connectUI;

    public UI_Mission ConnectUI { set { _connectUI = value; } }

    [SerializeField] List<Mission> _missionList = new List<Mission>();

    private void Awake()
    {
        Manager.Game.Mission = this;
    }

    private void Start()
    {
        _connectUI = Manager.Game.GameUI.GetComponent<UI_Mission>();
    }

    public Mission CurrentMission()
    {
        return _missionList[_nowMission];
    }
    
}
