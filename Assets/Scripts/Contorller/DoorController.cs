using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DoorController : InteracterController
{
    public override void Interact()
    {
        Debug.Log("문과 상호작용");
    }
}
