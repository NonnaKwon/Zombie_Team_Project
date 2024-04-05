using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : InteracterController
{
    UI_Shop _connectUI;

    protected override void Start()
    {
        base.Start();
        _connectUI = Manager.Resource.Load<UI_Shop>("Prefabs/UI/Popup/UI_Shop");

    }
    public override void Interact()
    {
        Manager.UI.ShowPopUpUI(_connectUI);
    }
}
