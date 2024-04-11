using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Prologue : PopUpUI
{
    private void Update()
    {
        if(Input.anyKey)
        {
            Close();
        }
    }
}
