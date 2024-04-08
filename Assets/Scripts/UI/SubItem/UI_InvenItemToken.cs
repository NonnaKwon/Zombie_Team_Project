using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InvenItemToken : BaseUI
{
    ItemData _item;
    int _count = 0;
    enum GameObjects
    {
        Image,
        Data,
        Slot,
        Count
    }

    private void Start()
    {
        
    }

    public void SetActiveToken(bool active)
    {
        GetUI(GameObjects.Data.ToString()).SetActive(active);
        GetUI(GameObjects.Slot.ToString()).SetActive(active);
    }

    public bool GetActiveTokwn()
    {
        return GetUI(GameObjects.Data.ToString()).activeSelf;
    }

    public void SetData(ItemData item,int count)
    {
        this._count = count;
        if (_item == null)
        {
            _item = item;
            InitData();
        }
        else
            UpdateData();

    }

    private void InitData()
    {
        GetUI<Image>(GameObjects.Image.ToString()).sprite = _item.image;
        GetUI<TMP_Text>(GameObjects.Count.ToString()).text = _count.ToString();
    }

    private void UpdateData()
    {
        GetUI<TMP_Text>(GameObjects.Count.ToString()).text = _count.ToString();
    }
}
