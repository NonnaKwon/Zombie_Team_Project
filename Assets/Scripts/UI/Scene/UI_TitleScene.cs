using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : MonoBehaviour
{
    public void OnClickStart()
    {
        Manager.Scene.LoadScene("GameScene");
    }
}
