using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : InGameUI
{
    private float _passTime = Define.PLAY_TIME;
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

    protected override void Awake()
    {
        base.Awake();
        Manager.Game.GameUI = this;

    }

    private void Start()
    {
        
    }
    private void Update()
    {
        _passTime -= Time.deltaTime;
        GetUI<TMP_Text>(GameObjects.Min.ToString()).text = ((int)_passTime / 60).ToString("D2");
        GetUI<TMP_Text>(GameObjects.Sec.ToString()).text = ((int)_passTime % 60).ToString("D2");
    }

    public void ChangeData(char state,float decreaseValue,bool isPlus = false)
    {
        string slider = "";
        switch (state)
        {
            case 'H':
                slider = GameObjects.HungerSlider.ToString();
                break;
            case 'T':
                slider = GameObjects.ThirstSlider.ToString();
                break;
            case 'F':
                slider = GameObjects.FatigueSlider.ToString();
                break;
            case 'S':
                slider = GameObjects.StaminaSlider.ToString();
                break;
        }

        if(isPlus)
            GetUI<Slider>(slider).value += decreaseValue;
        else
            GetUI<Slider>(slider).value -= decreaseValue;
    }

    public void SetMaxHP(float maxHp)
    {
        GetUI<Slider>(GameObjects.PlayerHP.ToString()).maxValue = maxHp;
        GetUI<Slider>(GameObjects.PlayerHP.ToString()).value = maxHp;
    }
    public void ChangeHP(float hp)
    {
        GetUI<Slider>(GameObjects.PlayerHP.ToString()).value = hp;
    }
}
