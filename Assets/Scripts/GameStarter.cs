using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : SoundManager
{
    public AudioClip backgroundMusic; // 인스펙터에서 할당

    void Start()
    {
        SoundManager.Instance.PlayBGM(backgroundMusic);
    }
}
