using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Mission : BaseUI
{
    float _currentCoin = 0;
    float _currentDeadBoss = 0;
    bool _missionComplete = false;

    Mission _currentMission;

    enum GameObjects
    {
        MissionContent,
        Percent,
        BossCount,
        CoinRec,
        BossRec,
        MissionList
    }

    protected override void Awake()
    {
        base.Awake();
        Manager.Game.Player.CoinChange -= PercentCal;
        Manager.Game.Player.CoinChange += PercentCal;
        Manager.Game.BossChange -= BossIncrease;
        Manager.Game.BossChange += BossIncrease;
    }

    private void Start()
    {
        GetUI<TMP_Text>(GameObjects.MissionContent.ToString()).text = _currentMission.mission;
        if (_currentMission.coinGoal == 0)
            GetUI(GameObjects.CoinRec.ToString()).SetActive(false);
        if (_currentMission.bossGoal == 0)
            GetUI(GameObjects.BossRec.ToString()).SetActive(false);
    }

    private void PercentCal(int coin)
    {
        _currentCoin = coin;
        GetUI<TMP_Text>(GameObjects.Percent.ToString()).text = ((int)(((float)_currentCoin / _currentMission.coinGoal) * 100f)).ToString("D2");
        IsComplete();
    }

    private void BossIncrease()
    {
        if (_missionComplete || _currentMission.bossGoal == 0)
            return;
        _currentDeadBoss = Manager.Game.BossCount;
        GetUI<TMP_Text>(GameObjects.BossCount.ToString()).text = _currentDeadBoss.ToString();
        IsComplete();
    }

    private void IsComplete()
    {
        if (_currentCoin >= _currentMission.coinGoal && _currentDeadBoss >= _currentMission.bossGoal)
        {
            Manager.Game.Mission.MissionComplete();
            TextColor(Color.green);
            _missionComplete = true;
        }
    }

    private void TextColor(Color color)
    {
        TMP_Text[] lists = GetUI(GameObjects.MissionList.ToString()).GetComponentsInChildren<TMP_Text>();
        foreach(TMP_Text text in lists)
        {
            text.color = color;
        }
    }

    public void SetCurrentMission(Mission mission)
    {
        _currentMission = mission;
    }
}
