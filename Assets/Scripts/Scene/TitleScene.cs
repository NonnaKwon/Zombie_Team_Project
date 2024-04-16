using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField] AudioClip _bgm;
    private void Start()
    {
        Manager.Sound.PlayBGM(_bgm);
    }
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
