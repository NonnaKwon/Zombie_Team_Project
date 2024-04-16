using UnityEngine;

public class InGameUI : BaseUI
{
    private void LateUpdate()
    {

    }

    public void Close()
    {
        Manager.UI.CloseInGameUI();
    }
}
