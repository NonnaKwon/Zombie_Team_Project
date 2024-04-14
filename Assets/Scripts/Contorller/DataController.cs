using System.IO;
using UnityEngine;

public class DataController : Singleton<DataController>
{
    private string dataPath;
    private PlayGameData gameData;

    void Awake()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadGameData();
    }

    public void LoadGameData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<PlayGameData>(json);
        }
        else
        {
            gameData = new PlayGameData();
        }
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(dataPath, json);
    }

    public int GetGold()
    {
        return gameData.gold;
    }

    public void AddGold(int amount)
    {
        gameData.gold += amount;
        SaveGameData();
    }
}

[System.Serializable]
public class PlayGameData
{
    public int gold = 0;
}