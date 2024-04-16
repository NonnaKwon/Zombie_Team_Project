using System;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements.Experimental;

public class DataController :MonoBehaviour
{
    void Start()
    {
        Manager.Data.LoadData();
        DataManager.Instance.AddGoldCoins(1);
        Manager.Data.SaveData();
        int loadCoin = Manager.Data.GameData.GoldCoins;
    }
    // 코인 획득
    public void EarnCoins(int coinsEarned)
    {
        DataManager.Instance.AddGoldCoins(coinsEarned);
    }

    // 코인 사용
    public void SpendCoins(int coinsSpent)
    {
        DataManager.Instance.AddGoldCoins(-coinsSpent);
    }



    // 데이터 리셋
    public void ResetGameData()
    {
        DataManager.Instance.NewData();
        DataManager.Instance.SaveData();
    }
}