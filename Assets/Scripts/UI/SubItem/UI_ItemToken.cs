using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemToken : BaseUI
{
    private ItemData _item;
    private int _count = 10;

    ShopController _connectShop;

    enum GameObjects
    {
        ItemImage,
        PriceText,
        ItemCount,
        BuyBtn,
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetItem(ItemData data, int count,ShopController shop)
    {
        _item = data;
        _connectShop = shop;
        _count = count;
        InitInfo();
    }


    private void InitInfo()
    {
        GetUI<Image>(GameObjects.ItemImage.ToString()).sprite = _item.image;
        GetUI<TMP_Text>(GameObjects.PriceText.ToString()).text = _item.price.ToString();
        GetUI<TMP_Text>(GameObjects.ItemCount.ToString()).text = _count.ToString();
    }

    private void UpdateInfo()
    {
        if (_count == 0)
            GetUI<TMP_Text>(GameObjects.ItemCount.ToString()).text = "매진";
        else
            GetUI<TMP_Text>(GameObjects.ItemCount.ToString()).text = _count.ToString();
    }

    public void Buy()
    {
        if (Manager.Game.Player.Coin < _item.price)
        {
            Debug.Log("돈이 모자람");
            return;
        }

        if(_count == 0)
        {
            Debug.Log("매진");
            return;
        }

        Manager.Game.Player.Coin -= _item.price;
        Manager.Game.Player.GetComponent<Inventory>().AddItem(_item);

        _count--;
        _connectShop.UpdateData(_item, _count);
        UpdateInfo();
        Debug.Log(_item.name + "샀다");
    }

    public void BuyClick()
    {
        UI_BuyPopup buyPopup = Manager.Resource.Load<UI_BuyPopup>("Prefabs/UI/Popup/UI_BuyPopup");
        buyPopup.Token = this;
        Manager.UI.ShowPopUpUI(buyPopup);
    }
    
}
