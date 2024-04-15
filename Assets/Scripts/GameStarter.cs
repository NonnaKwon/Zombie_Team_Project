using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : SoundManager
{
    public AudioClip backgroundMusic; // �ν����Ϳ��� �Ҵ�

    void Start()
    {
        SoundManager.Instance.PlayBGM(backgroundMusic);
    }
}
