using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    private float _passTime = PLAY_TIME;

    GameManager _game;
    UI_Prologue _prologue;
    PooledObject _muzzleFlash;
    PooledObject _explosion;
    PooledObject _bloodEffect;
    PooledObject _fireBlood;
    private void Start()
    {
        _game = Manager.Game;
        _prologue = Manager.Resource.Load<UI_Prologue>("Prefabs/UI/Popup/UI_Prologue");
        _muzzleFlash = Manager.Resource.Load<PooledObject>("Prefabs/Effects/MuzzleFlash");
        _explosion = Manager.Resource.Load<PooledObject>("Prefabs/Effects/Explosion");
        _bloodEffect = Manager.Resource.Load<PooledObject>("Prefabs/Effects/BloodEffect");
        _fireBlood = Manager.Resource.Load<PooledObject>("Prefabs/Effects/FireBloodEffect");

        Manager.Pool.CreatePool(_muzzleFlash, 10, 10);
        Manager.Pool.CreatePool(_explosion, 5, 5);
        Manager.Pool.CreatePool(_bloodEffect, 20, 20);
        Manager.Pool.CreatePool(_fireBlood, 10, 10);
    }

    private void Update()
    {
        _passTime -= Time.unscaledDeltaTime;
        _game.GameUI.PassTime = _passTime;
        if (_passTime <= 0)
        {
            if (_game.Player.Coin < 5000)
                _game.ShowEnding(EndingType.GroundZero);
            else if (_game.Player.Coin < 10000) // && 그라운드 제로 엔딩 해금이면. (구현되면 추가할 것)
                _game.ShowEnding(EndingType.Hope);
            else if (_game.ZombieCount < 500) // && 희망 엔딩 해금이면
                _game.ShowEnding(EndingType.HopeFromDespair);
            else // && 희망 엔딩 해금이면
                _game.ShowEnding(EndingType.DespairFromHope);
        }
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(7f);
        Manager.UI.ShowPopUpUI(_prologue);
    }
}
