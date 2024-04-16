using System;
using UnityEngine;
using static Define;

public class GameManager : Singleton<GameManager>
{
    private PlayerController _player;
    private MissionController _mission;
    private UI_GameScene _gameUI;

    private int _bossCount = 0;

    public bool IsSpawn = false;
    public event Action BossChange;
    public int BossCount { get { return _bossCount; } set { _bossCount = value; BossChange?.Invoke(); } }
    public PlayerController Player 
    { get
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            return _player;
        } 
        set { _player = value; } 
    }

    public UI_GameScene GameUI
    {
        get
        {
            if (_gameUI == null)
                _gameUI = GameObject.FindGameObjectWithTag("GameUI").GetComponent<UI_GameScene>();
            return _gameUI;
        }
        set
        {
            _gameUI = value;
        }
    }
    public MissionController Mission { get { return _mission; } set { _mission = value; } }

    

    private void Start()
    {
        
    }

    public void ShowEnding(EndingType endingType)
    {
        Manager.Scene.LoadEndingScene("EndingScene", endingType);
    }



}
