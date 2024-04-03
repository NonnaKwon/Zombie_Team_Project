using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController Player 
    { get
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            return _player;
        } 
        set { _player = value; } 
    }

    public UI_GameScene GameUI { get; set; }

    private PlayerController _player;
    public void Test()
    {
        Debug.Log(GetInstanceID());
    }
}
