using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] GameObject _bossPrefab;

    private float _passTime = PLAY_TIME;
    private bool isBoss = false;

    GameManager _game;
    UI_Prologue _prologue;
    PooledObject _muzzleFlash;
    PooledObject _explosion;
    PooledObject _bloodEffect;
    PooledObject _fireBlood;
    PooledObject _crawlZombie;
    PooledObject _runZombie;
    PooledObject _walkZombie;
    PooledObject _coin;

    private void Start()
    {
        _game = Manager.Game;
        _prologue = Manager.Resource.Load<UI_Prologue>("Prefabs/UI/Popup/UI_Prologue");
        _muzzleFlash = Manager.Resource.Load<PooledObject>("Prefabs/Effects/MuzzleFlash");
        _explosion = Manager.Resource.Load<PooledObject>("Prefabs/Effects/Explosion");
        _bloodEffect = Manager.Resource.Load<PooledObject>("Prefabs/Effects/BloodEffect");
        _fireBlood = Manager.Resource.Load<PooledObject>("Prefabs/Effects/FireBloodEffect");
        _crawlZombie = Manager.Resource.Load<PooledObject>("Prefabs/CrawlZombie");
        _runZombie = Manager.Resource.Load<PooledObject>("Prefabs/RunZombie");
        _walkZombie = Manager.Resource.Load<PooledObject>("Prefabs/WalkZombie");
        _coin = Manager.Resource.Load<PooledObject>("Prefabs/GoldCoins");

        Manager.Pool.CreatePool(_muzzleFlash, 10, 10);
        Manager.Pool.CreatePool(_explosion, 5, 5);
        Manager.Pool.CreatePool(_coin, 5, 5);
        Manager.Pool.CreatePool(_bloodEffect, 20, 20);
        Manager.Pool.CreatePool(_fireBlood, 10, 10);
        Manager.Pool.CreatePool(_crawlZombie, ZOMBIE_POOL_SIZE, ZOMBIE_POOL_SIZE);
        Manager.Pool.CreatePool(_runZombie, ZOMBIE_POOL_SIZE, ZOMBIE_POOL_SIZE);
        Manager.Pool.CreatePool(_walkZombie, ZOMBIE_POOL_SIZE, ZOMBIE_POOL_SIZE);
    }

    private void Update()
    {
        _passTime -= Time.unscaledDeltaTime;
        _game.GameUI.PassTime = _passTime;

        if (!Manager.Game.IsSpawn && _passTime <= PLAY_TIME - SPAWN_TIME)
            Manager.Game.IsSpawn = true;

        if (!isBoss && _passTime <= PLAY_TIME - BOSS_PLAY_TIME)
        {
            Vector3 insPos = _game.Player.transform.position + new Vector3(5f,0,5f);
            Instantiate(_bossPrefab, insPos, _game.transform.rotation);
            isBoss = true;
        }
        else if (_passTime <= 0)
        {
            if (_game.Player.Coin < 5000)
                _game.ShowEnding(EndingType.GroundZero);
            else if (_game.Player.Coin < 10000)
                _game.ShowEnding(EndingType.Hope);
            else if (_game.BossCount < 3) //수정해야함
                _game.ShowEnding(EndingType.HopeFromDespair);
            else
                _game.ShowEnding(EndingType.DespairFromHope);
        }
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(7f);
        Manager.UI.ShowPopUpUI(_prologue);
    }
}
