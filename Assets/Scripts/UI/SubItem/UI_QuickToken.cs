using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickToken : BaseUI
{
    ItemData _item;
    int _count;
    enum GameObjects
    {
        Slot,
        Count,
        WeaponCount
    }

    private void Start()
    {

    }

    public void SetActiveToken(bool active)
    {
        GetUI(GameObjects.Slot.ToString()).SetActive(active);
        GetUI(GameObjects.Count.ToString()).SetActive(active);
        GetUI(GameObjects.WeaponCount.ToString()).SetActive(active);
        if(active)
        {
            if(_item as WeaponData != null)
                GetUI(GameObjects.Count.ToString()).SetActive(false);
            else
                GetUI(GameObjects.WeaponCount.ToString()).SetActive(false);
        }
    }

    public void SetData(ItemData data,int count)
    {
        _item = data;
        _count = count;
        GetUI<Image>(GameObjects.Slot.ToString()).sprite = _item.image;
        GetUI<TMP_Text>(GameObjects.Count.ToString()).text = _count.ToString();
    }

    public void DecreaseCount()
    {
        if (!GetActiveToken())
            return;
        _count--;
        if (_count > 0)
            GetUI<TMP_Text>(GameObjects.Count.ToString()).text = _count.ToString();
        else
            SetActiveToken(false);
    }

    public bool GetActiveToken()
    {
        return GetUI(GameObjects.Slot.ToString()).activeSelf;
    }
}
