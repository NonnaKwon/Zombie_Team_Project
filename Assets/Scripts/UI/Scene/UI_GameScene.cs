using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : InGameUI
{
    private float _passTime = Define.PLAY_TIME;
    private UI_QuickSlot _quickSlot;
    public UI_QuickSlot QuickSlot { get { return _quickSlot; } }
    enum GameObjects
    {
        PlayerHP,
        HungerSlider,
        ThirstSlider,
        FatigueSlider,
        StaminaSlider,
        Sec,
        Min,
        Mission,
        Coin
    }

    protected override void Awake()
    {
        base.Awake();
        Manager.Game.GameUI = this;
        _quickSlot = GetComponentInChildren<UI_QuickSlot>();

    }

    private void Start()
    {
        Manager.Game.Player.CoinChange -= SetCoin;
        Manager.Game.Player.CoinChange += SetCoin;
    }
    private void Update()
    {
        _passTime -= Time.deltaTime;
        GetUI<TMP_Text>(GameObjects.Min.ToString()).text = ((int)_passTime / 60).ToString("D2");
        GetUI<TMP_Text>(GameObjects.Sec.ToString()).text = ((int)_passTime % 60).ToString("D2");
    }

    public void AddQuickSlot(ItemData item,int count)
    {
        _quickSlot.AddQuickSlot(item, count);
    }
    public void SetCoin(int amount)
    {
        GetUI<TMP_Text>(GameObjects.Coin.ToString()).text = amount.ToString();
    }

    public void ChangeData(char state,float value)
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
        GetUI<Slider>(slider).value = value;
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
