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
    public MissionController Mission { get { return _mission; } set { _mission = value; } }

    private PlayerController _player;
    private MissionController _mission;

    private void Start()
    {
        _mission.ConnectUI = GameUI.GetComponentInChildren<UI_Mission>();
    }

    

}
