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
        Debug.Log("문과 상호작용");
        Manager.Game.Player.StateMachine.ChangeState(PlayerState.Idle);
    }
}
