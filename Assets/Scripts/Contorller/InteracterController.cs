using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Define;

public abstract class InteracterController : MonoBehaviour, IInteractable
{
    Vector3 _offset = new Vector3(0, 1f, 0);
    //랜덤으로 아이템 리스트를
    bool _isActive;
    GameObject _uIData;
    [SerializeField] RectTransform _btnInfo;


    protected virtual void Start()
    {
        _isActive = false;
        _uIData = GetComponentInChildren<Canvas>().gameObject;
        _uIData.SetActive(false);
    }

    protected void LateUpdate()  
    {
        if (_isActive)
            _btnInfo.position = Camera.main.WorldToScreenPoint(transform.position + _offset);
    }

    public void OnActive()
    {
        _isActive = true;
        _uIData.SetActive(_isActive);
    }

    public void OffActive()
    {
        _isActive = false;
        _uIData.SetActive(_isActive);
    }
    public abstract void Interact();

    protected void OnInteract(InputValue value)
    {
        if (_isActive && Manager.Game.Player.StateMachine.CurState != PlayerState.Interact)
        {
            Manager.Game.Player.StateMachine.ChangeState(PlayerState.Interact);
            Interact();
        }
    }
}
