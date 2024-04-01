using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void OnActive();
    public void OffActive();
}
