public class PopUpUI : BaseUI
{
    public void Close(bool isIdle = true)
    {
        Manager.UI.ClosePopUpUI(isIdle);
    }
}
