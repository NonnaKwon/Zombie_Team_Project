using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class UI_Mission : BaseUI
{
    float _currentCoin = 0;
    Mission _currentMission;
    enum GameObjects
    {
        MissionContent,
        Percent
    }

    private void Start()
    {
        _currentMission = Manager.Game.Mission.CurrentMission();
        GetUI<TMP_Text>(GameObjects.MissionContent.ToString()).text = _currentMission.mission;
        Manager.Game.Player.CoinChange -= PercentCal;
        Manager.Game.Player.CoinChange += PercentCal;
    }                  

    private void PercentCal(int coin)
    {
        GetUI<TMP_Text>(GameObjects.Percent.ToString()).text = (((float)_currentCoin/_currentMission.coin) * 100f).ToString();
    }
}
