using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController Player { get; set; }
    public UI_GameScene GameUI { get; set; }
    public void Test()
    {
        Debug.Log(GetInstanceID());
    }
}
