using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InvenItemToken : BaseUI, IPointerClickHandler
{
    ItemData _item;
    int _count = 0;
    bool _isQuickAdd = false;
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

    public bool GetActiveToken()
    {
        return GetUI(GameObjects.Data.ToString()).activeSelf;
    }

    public void SetData(ItemData item,int count)
    {
        if (count == 0)
        {
            SetActiveToken(false);
            return;
        }
        _count = count;
        _item = item;
        InitData();

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isQuickAdd && eventData.button == PointerEventData.InputButton.Right)
        {
            Manager.Game.GameUI.AddQuickSlot(_item,_count);
            _isQuickAdd = true;
        }
    }

}
