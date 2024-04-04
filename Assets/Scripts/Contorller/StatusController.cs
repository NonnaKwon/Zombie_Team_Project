using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatusController : MonoBehaviour
{
    enum Status
    {
        Hunger,
        Thirst,
        Fatigue,
        Stamina
    }

    UI_GameScene _connectUI;
    FightController _fightController;
    PlayerController _playerController;

    private float _passTime = 0;
    private float _dashTime = 0;
    private bool _onDash = false;

    private float _hunger = 1;
    private float _thirst = 1;
    private float _fatigue = 1;
    private float _stamina = 1;
    private float _decreaseStaminaAmount = 0;

    private int _step = 3; //디버프 효과 단계
    private float[] _thirstRatio = { 0.7f, 0.5f, 0.2f };
    private float[] _thirstEffectRatio = { 0.05f, 0.1f, 0.2f };

    private float[] _fatigueRatio = { 0.7f, 0.4f, 0.1f };
    private float[] _fatigueEffectRatio = { 0.02f, 0.05f, 0.08f };
    private float _staminaEffectRatio = 0.01f;

    private const float DECREASE = 0.04f / 60; //분당 2% 여서 초당으로 계산.
    private const float DECREASE_STAMINA = 0.05f;
    private const float DECREASE_STAMINA_FATIGUE = 0.01f;

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
        _passTime += Time.deltaTime;
        if (_onDash)
        {
            _dashTime += Time.deltaTime;
            _dashTime = 0;
            ChangeData(Status.Stamina, DECREASE_STAMINA / _frame);

        }

        _passTime = 0;
        ChangeData(Status.Hunger,  DECREASE / _frame);
        ChangeData(Status.Thirst,  DECREASE / _frame);
        ChangeData(Status.Fatigue, DECREASE / _frame);

        UpdateStateEffect();
    }

    private void ChangeData(Status state, float decreaseValue)
    {
        switch (state)
        {
            case Status.Hunger:
                _hunger -= decreaseValue;
                _connectUI.ChangeData('H', decreaseValue);
                break;
            case Status.Thirst:
                _thirst -= decreaseValue;
                _connectUI.ChangeData('T', decreaseValue);
                break;
            case Status.Fatigue:
                _fatigue -= decreaseValue;
                _connectUI.ChangeData('F', decreaseValue);
                break;
            case Status.Stamina:
                _stamina -= decreaseValue;
                _decreaseStaminaAmount += decreaseValue;
                _connectUI.ChangeData('S', decreaseValue);
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
                _fightController.AttackSpeed -= _fightController.AttackSpeed * _thirstEffectRatio[i];
                break;
            }
        }

        for (int i =_step-1; i >= 0; i--)
        {
            if (_fatigue < _fatigueRatio[i])
            {
                _playerController.MoveSpeed -= _playerController.MoveSpeed * _fatigueEffectRatio[i];
                break;
            }
        }

        if (_decreaseStaminaAmount >= DECREASE_STAMINA)
        {
            Debug.Log("피로도 감소 ");
            ChangeData(Status.Fatigue, _staminaEffectRatio);
            _decreaseStaminaAmount = 0;
        }
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
            _onDash = true;
        else
        {
            _onDash = false;
            _dashTime = 0;
        }
    }

}
