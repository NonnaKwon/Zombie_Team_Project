using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour
{
    private Vector3 _moveDir;
    private float _moveSpeed;
    private float _jumpPower;
    private float _dashSpeed;

    private float _curSpeed;
    private int _stackJumpCount;
    private bool _canMove;

    Rigidbody _rigid;
    [SerializeField] LayerMask _groundFind;

    private StateMachine<PlayerState> _stateMachine;
    public StateMachine<PlayerState> StateMachine { get { return _stateMachine; } }


    private void Awake()
    {
        //StateMachine
        _stateMachine = new StateMachine<PlayerState>();
        _stateMachine.AddState(PlayerState.Idle, new IdleState(this));
        _stateMachine.AddState(PlayerState.Interact, new InteractState(this));
        _stateMachine.AddState(PlayerState.Die, new DieState(this));
        _stateMachine.Start(PlayerState.Idle);
        
        //Component
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        PlayerInit();
    }

    public void PlayerInit()
    {
        _canMove = true;
        _stackJumpCount = 0;
        _moveSpeed = 8f;
        _jumpPower = 7f;
        _dashSpeed = 14f;
        _curSpeed = _moveSpeed;
        _groundFind = LayerMask.GetMask("Ground");
        if (_stateMachine.CurState != PlayerState.Idle)
            _stateMachine.ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        _stateMachine.Update();
        if (_canMove)
            Move();
    }
    private void Move()
    {
        transform.Translate(_moveDir * _curSpeed * Time.deltaTime);
    }


    private void Jump()
    {
        _stackJumpCount++;
        _rigid.velocity = new Vector3(_rigid.velocity.x, _jumpPower, _rigid.velocity.z);
    }


    private void OnJump(InputValue value)
    {
        if (!_canMove)
            return;
        if (_stackJumpCount < JUMP_MAX_COUNT)
            Jump();
    }

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
            _curSpeed = _dashSpeed;
        else
            _curSpeed = _moveSpeed;

    }

    private void OnMove(InputValue value)
    {
        Vector2 moveDistance = value.Get<Vector2>();
        _moveDir.x = moveDistance.x;
        _moveDir.z = moveDistance.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_groundFind.Contain(collision.gameObject.layer))
        {
            _stackJumpCount = 0;
        }
    }


    private class PlayerStateClass : BaseState<PlayerState>
    {
        protected PlayerController owner;
        public PlayerStateClass(PlayerController owner)
        {
            this.owner = owner;
        }
    }

    private class IdleState : PlayerStateClass
    {
        public IdleState(PlayerController owner) : base(owner) { }

        public override void Update()
        {

        }

    }


    private class InteractState : PlayerStateClass
    {
        public InteractState(PlayerController owner) : base(owner) { }

        public override void Enter()
        {

        }
        public override void Transition()
        {

        }

    }


    private class DieState : PlayerStateClass
    {
        public DieState(PlayerController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Player : Die");
        }

        public override void Transition()
        {

        }

        public override void Exit()
        {
            Manager.UI.ClearPopUpUI();
        }
    }
}
