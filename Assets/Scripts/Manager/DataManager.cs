using System;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private GameData gameData;
    public GameData GameData { get { return gameData; } }

#if UNITY_EDITOR
    private string path => Path.Combine(Application.dataPath, $"Resources/Data/SaveLoad");
#else
    private string path => Path.Combine(Application.persistentDataPath, $"Resources/Data/SaveLoad");
#endif

    public void NewData()
    {
        gameData = new GameData();
    }

    public void SaveData(int index = 0)
    {
        Debug.Log("���� �Ϸ�");
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText($"{path}/{index}.txt", json);
    }

    public void LoadData(int index = 0)
    {
        if (File.Exists($"{path}/{index}.txt") == false)
        {
            NewData();
            return;
        }

        string json = File.ReadAllText($"{path}/{index}.txt");
        try
        {
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
            NewData();
        }
    }
    public bool ExistData(int index = 0)
    {
        return File.Exists($"{path}/{index}.txt");
    }

    public void AddGoldCoins(int amount)
    {
        gameData.GoldCoins += amount;
        SaveData(0);
    }

    public int GetGoldCoins()
    {
        return gameData.GoldCoins;
    }

    public void ResetGameData()
    {
        DataManager.Instance.NewData();
        DataManager.Instance.SaveData();
    }
}
