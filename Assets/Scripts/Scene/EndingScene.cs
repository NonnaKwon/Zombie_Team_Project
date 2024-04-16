using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class EndingScene : BaseScene
{
    public EndingType Ending;
    public UI_EndingScene ConnectUI;
    AudioClip _audio;
    private void Awake()
    {
        _audio = Manager.Resource.Load<AudioClip>("Sounds/cinematic");
    }

    public override IEnumerator LoadingRoutine()
    {
        ConnectUI.SetImage(Ending);
        ConnectUI.FlowEnding();
        Manager.Sound.PlayBGM(_audio);
        yield return null;
    }

    
}
