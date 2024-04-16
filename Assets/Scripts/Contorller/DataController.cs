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
    // ���� ȹ��
    public void EarnCoins(int coinsEarned)
    {
        DataManager.Instance.AddGoldCoins(coinsEarned);
    }

    // ���� ���
    public void SpendCoins(int coinsSpent)
    {
        DataManager.Instance.AddGoldCoins(-coinsSpent);
    }



    // ������ ����
    public void ResetGameData()
    {
        DataManager.Instance.NewData();
        DataManager.Instance.SaveData();
    }
}