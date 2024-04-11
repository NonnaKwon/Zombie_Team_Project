using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class StatusController : MonoBehaviour
{

    UI_GameScene _connectUI;
    FightController _fightController;
    PlayerController _playerController;

    private bool _onDash = false;

    private float _hunger = 1;
    private float _thirst = 1; //테스트
    private float _fatigue = 1;
    private float _stamina = 1;
    private float _decreaseStaminaAmount = 0;

    private int _step = 3; //디버프 효과 단계
    private float[] _thirstRatio = { 0.7f, 0.5f, 0.2f };
    private float[] _thirstEffectRatio = { 0.05f, 0.1f, 0.2f };

    private float[] _fatigueRatio = { 0.7f, 0.4f, 0.1f };
    private float[] _fatigueEffectRatio = { 0.02f, 0.05f, 0.08f };
    private float _staminaEffectRatio = 0.01f;

    private const float DECREASE = 0.04f / 60; //분당이어서 초당으로 계산.
    private const float DECREASE_STAMINA = 0.05f;
    private const float DECREASE_STAMINA_FATIGUE = 0.01f;
    private const float INCREASE_STAMINA = 0.03f;

    private float _frame;

    // Start is called before the first frame update
    void Start()
    {
        _connectUI = Manager.Game.GameUI;
        _frame = 1 / Time.deltaTime;

        _playerController = Manager.Game.Player;
        _fightController = _playerController.gameObject.GetComponent<FightController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_onDash)
            ChangeData(Status.Stamina, DECREASE_STAMINA / _frame);
        else
            ChangeData(Status.Stamina, INCREASE_STAMINA / _frame, true);
        ChangeData(Status.Hunger,  DECREASE / _frame);
        ChangeData(Status.Thirst,  DECREASE / _frame);
        ChangeData(Status.Fatigue, DECREASE / _frame);

        UpdateStateEffect();
    }

    public void ChangeData(Status state, float value,bool isPlus = false)
    {
        switch (state)
        {
            case Status.Hunger:
                _hunger += isPlus ? value : -value;
                _connectUI.ChangeData('H', _hunger);
                break;
            case Status.Thirst:
                _thirst += isPlus ? value : -value;
                _connectUI.ChangeData('T', _thirst);
                break;
            case Status.Fatigue:
                _fatigue += isPlus ? value : -value;
                _connectUI.ChangeData('F', _fatigue);
                break;
            case Status.Stamina:
                _stamina += isPlus ? value : -value;
                if(!isPlus)
                    _decreaseStaminaAmount += value;
                _connectUI.ChangeData('S', _stamina);
                break;
        }
    }

    private void UpdateStateEffect()
    {
        if (_hunger <= 0)
            Manager.Game.Player.StateMachine.ChangeState(Define.PlayerState.Die);

        for(int i=_step-1; i>=0;i--)
        {
            if(_thirst < _thirstRatio[i])
            {
                _fightController.AttackSpeed = _fightController.AttackSpeedBase - _fightController.AttackSpeedBase * _thirstEffectRatio[i];
                break;
            }
        }

        for (int i =_step-1; i >= 0; i--)
        {
            if (_fatigue < _fatigueRatio[i])
            {
                _playerController.CurSpeed = _playerController.MoveSpeed - _playerController.MoveSpeed * _fatigueEffectRatio[i];
                break;
            }
        }

        if (_decreaseStaminaAmount >= DECREASE_STAMINA)
        {
            ChangeData(Status.Fatigue, _staminaEffectRatio);
            _decreaseStaminaAmount = 0;
        }
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
            _onDash = true;
        else
            _onDash = false;
    }

}
