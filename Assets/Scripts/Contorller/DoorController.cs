using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Define;

public class DoorController : InteracterController
{
    public override void Interact()
    {
        Debug.Log("���� ��ȣ�ۿ�");
        Manager.Game.Player.StateMachine.ChangeState(PlayerState.Idle);
    }
}
