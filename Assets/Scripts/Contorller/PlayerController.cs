using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour
{
    private Vector3 _moveDir;
    private float _moveSpeed;
    private float _dashSpeed;

    private float _curSpeed;
    private bool _canMove;
    private bool _onMouseRotate;

    Rigidbody _rigid;
    Vector2 _mousePos;
    LayerMask _groundFind;

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
        _onMouseRotate = false;
        _canMove = true;
        _moveSpeed = 8f;
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
        {
            Rotate();
            Move();
        }
    }

    private void Rotate()
    {
        if (_onMouseRotate)
        {
            Ray ray = Camera.main.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 0.5f);

                Vector3 tmpDir = hit.point - transform.position;
                Vector3 rotateDir = new Vector3(tmpDir.x, 0, tmpDir.z);
                Quaternion lookRotation = Quaternion.LookRotation(rotateDir);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
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
    }
    private void Move()
    {
        transform.Translate(_moveDir * _curSpeed * Time.deltaTime,Space.World);
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
