using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_Prologue _prologue;
    PooledObject _muzzleFlash;
    private void Start()
    {
        _prologue = Manager.Resource.Load<UI_Prologue>("Prefabs/UI/Popup/UI_Prologue");
        _muzzleFlash = Manager.Resource.Load<PooledObject>("Prefabs/Effects/MuzzleFlash");
        Manager.Pool.CreatePool(_muzzleFlash, 10, 10);
    }
    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(7f);
        Manager.UI.ShowPopUpUI(_prologue);
    }
}
