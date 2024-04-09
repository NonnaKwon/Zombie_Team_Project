using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuyPopup : PopUpUI
{
    public UI_ItemToken Token;
    
    enum GameObjects
    {

    }

    public void OnClickOK()
    {
        Token.Buy();
        Close(false);
    }
}
