using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_Prologue _prologue;
    PooledObject _muzzleFlash;
    PooledObject _explosion;
    PooledObject _bloodEffect;
    PooledObject _fireBlood;
    private void Start()
    {
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
    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(7f);
        Manager.UI.ShowPopUpUI(_prologue);
    }
}
