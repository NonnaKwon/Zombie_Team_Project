using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetItemToken : BaseUI
{
    ItemData _item;
    int _count = 0;

    enum GameObjects
    {
        Image,
        Count
    }

    public void SetData(ItemData item, int count)
    {
        _count = count;
        _item = item;
        InitData();
    }

    private void InitData()
    {
        GetUI<Image>(GameObjects.Image.ToString()).sprite = _item.image;
        GetUI<TMP_Text>(GameObjects.Count.ToString()).text = _count.ToString();
    }

}
