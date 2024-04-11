using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_Prologue _prologue;
    private void Start()
    {
        _prologue = Manager.Resource.Load<UI_Prologue>("Prefabs/UI/Popup/UI_Prologue");
    }
    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(7f);
        Manager.UI.ShowPopUpUI(_prologue);
    }
}
