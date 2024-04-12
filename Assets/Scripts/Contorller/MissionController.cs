using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    private int _nowMission;
    UI_Mission _connectUI;
    public UI_Mission ConnectUI { set { _connectUI = value; } }

    [Serializable]
    private class Mission
    {
        public int id;
        public string mission;
        public bool isComplete;
    }

    [SerializeField] List<Mission> _missionList = new List<Mission>();

    private void Awake()
    {
        Manager.Game.Mission = this;
    }

    public string CurrentMission()
    {
        return _missionList[_nowMission].mission;
    }
    
}
