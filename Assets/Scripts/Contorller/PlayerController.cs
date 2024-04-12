using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

using static Define;

public class PlayerController : MonoBehaviour
{
    public GameObject MousePointer;

    private Vector3 _moveDir;

    private float _moveSpeed;
    private float _dashSpeedPercent;

    private float _curSpeed;
    private bool _canMove;
    private bool _onMouseRotate;
    private int _animationLayer;
    private bool _onDash;
    private int _coin;

    Rigidbody _rigid;
    Vector2 _mousePos;
    Animator _animator;

    private StateMachine<PlayerState> _stateMachine;
    public StateMachine<PlayerState> StateMachine { get { return _stateMachine; } }
    public Vector2 MousePos { get { return _mousePos; } }
    public float MoveSpeed { get { return _moveSpeed; } }
    public float CurSpeed { set { _curSpeed = value; } }
    public bool CanMove { get { return _canMove; } }
    public int Coin { get { return _coin; } 
        set 
        { 
            _coin = value;
            CoinChange?.Invoke(_coin);
        } 
    }

    public event Action<int> CoinChange;

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
        _animator = GetComponentInChildren<Animator>();
        
        Manager.Game.Player = this;
    }

    private void Start()
    {
        PlayerInit();


    }

    public void PlayerInit()
    {
        _onDash = false;
        _onMouseRotate = false;
        _canMove = true;
        _moveSpeed = 4f;
        _dashSpeedPercent = 1.8f;
        _curSpeed = _moveSpeed;
        _animationLayer = 0;
        Coin = 10000;

        if (_stateMachine.CurState != PlayerState.Idle)
            _stateMachine.ChangeState(PlayerState.Idle);
    }

    private void Update()
    {
        _stateMachine.Update();
        if (_canMove)
        {
            Rotate();
            Move();
        }
    }

    public void ChangeAnimationLayer(string name)
    {
        if (_animationLayer != 0)
            _animator.SetLayerWeight(_animationLayer, 0);
        _animationLayer = _animator.GetLayerIndex(name);
        _animator.SetLayerWeight(_animationLayer, 1);
    }


    private void Rotate()
    {
        if (_onMouseRotate)
        {
            Ray ray = Camera.main.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 tmpDir = hit.point - transform.position;
                Vector3 rotateDir = new Vector3(tmpDir.x, 0, tmpDir.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateDir), 10f * Time.deltaTime);
            }
        }
        else
        {
            Vector3 forwardDir = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            Vector3 rightDir = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

            if (_moveDir.sqrMagnitude > 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(forwardDir * _moveDir.z + rightDir * _moveDir.x);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }
        }
        MousePointer.transform.position = transform.position + transform.forward + new Vector3(0,1f,0);
    }
    private void Move()
    {
        float speed = _onDash ? _curSpeed * _dashSpeedPercent : _curSpeed;
        transform.Translate(_moveDir * speed * Time.deltaTime,Space.World);
        _animator.SetFloat("velocity", (_moveDir * speed).magnitude);  
        _animator.SetFloat("moveAngle", Extension.GetAngle(transform.forward, _moveDir)); //moveDir랑 지금 캐릭터가 보고있는 forward 각도 계산
    }


    private void OnDash(InputValue value)
    {
        if (value.isPressed)
            _onDash = true;
        else
            _onDash = false;
    }

    private void OnMove(InputValue value)
    {
        Vector2 moveDistance = value.Get<Vector2>();
        _moveDir.x = moveDistance.x;
        _moveDir.z = moveDistance.y;
    }


    private void OnMousePos(InputValue value)
    {
        _mousePos = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _onMouseRotate = value.isPressed ? true : false;
    }



    private void OnTriggerEnter(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact != null)
        {
            interact.OnActive();
            return;
        }

        MapController map = other.GetComponent<MapController>();
        if (map != null)
        {
            map.SpawnMap();
            return;
        }

        if (other.gameObject.CompareTag("Item")) // 내가 추가한 스크립트
        {
            Coin = Coin + Define.GET_COIN_AMOUNT;
            Destroy(other.gameObject); // 아이템을 게임 세계에서 제거
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interact = other.GetComponent<IInteractable>();
        if (interact != null)
        {
            interact.OffActive();
            return;
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

        public override void Enter()
        {
            owner._canMove = true;
        }

        public override void Update()
        {

        }

    }


    private class InteractState : PlayerStateClass
    {
        public InteractState(PlayerController owner) : base(owner) { }

        public override void Enter()
        {
            owner._canMove = false;
        }
        public override void Transition()
        {

        }

        public override void Exit()
        {
            owner._canMove = true;
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
