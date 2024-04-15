using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class EndingScene : BaseScene
{
    public EndingType Ending;
    public UI_EndingScene ConnectUI;

    public override IEnumerator LoadingRoutine()
    {
        ConnectUI.SetImage(Ending);
        yield return ConnectUI.FlowEnding();
    }

    
}
