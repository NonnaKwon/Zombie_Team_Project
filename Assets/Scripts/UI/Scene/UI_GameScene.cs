using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameScene : InGameUI
{
    private float _passTime = 0;
    enum GameObjects
    {
        PlayerHP,
        HungerSlider,
        ThirstSlider,
        FatigueSlider,
        StaminaSlider,
        Sec,
        Min,
        Mission
    }

    private void Start()
    {

    }

    private void Update()
    {
        _passTime += Time.deltaTime;
        GetUI<TMP_Text>(GameObjects.Min.ToString()).text = ((int)_passTime / 60).ToString("D2");
        GetUI<TMP_Text>(GameObjects.Sec.ToString()).text = ((int)_passTime % 60).ToString("D2");
    }

}
