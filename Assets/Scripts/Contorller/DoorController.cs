using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Define;

public class DoorController : InteracterController
{
    [SerializeField] StructureType _type;
    private bool _isGet = false;
    private UI_GetPopup _getPopup;

    private class ItemEntity
    {
        public Item item;
        public int count;
        public ItemEntity(Item item,int count)
        {
            this.item = item;
            this.count = count;
        }
    }

    List<ItemEntity> _existingItems;
    Item[] _randomItem = new Item[4] { new Coffee(), new EnergyBar(), new Water(), new CannedFood() };

    private void Awake()
    {
        _existingItems = new List<ItemEntity>();
        switch(_type)
        {
            case StructureType.cottage:
                RandomItemAdd(1, 2);
                _existingItems.Add(new ItemEntity(new GunItem(), 1));
                _existingItems.Add(new ItemEntity(new Grenade(), Random.Range(2, 5)));
                break;
            case StructureType.church:
                RandomItemAdd(2, 3);
                _existingItems.Add(new ItemEntity(new BatItem(), 1));
                break;
            case StructureType.apartment:
                RandomItemAdd(5, 7);
                _existingItems.Add(new ItemEntity(new BatItem(), 1));
                _existingItems.Add(new ItemEntity(new Bullet(), Random.Range(70, 100)));
                break;
            case StructureType.villa:
                RandomItemAdd(1, 2);
                _existingItems.Add(new ItemEntity(new GunItem(), 1));
                _existingItems.Add(new ItemEntity(new Bullet(), Random.Range(10, 30)));
                break;
            case StructureType.school:
                RandomItemAdd(5, 7);
                break;
            case StructureType.mall:
                RandomItemAdd(5, 7);
                _existingItems.Add(new ItemEntity(new GunItem(), 1));
                _existingItems.Add(new ItemEntity(new Bullet(), Random.Range(10, 30)));
                break;
            case StructureType.hospital:
                RandomItemAdd(2, 3);
                _existingItems.Add(new ItemEntity(new BatItem(), 1));
                _existingItems.Add(new ItemEntity(new Grenade(), Random.Range(2, 5)));
                break;
        }
        _getPopup = Manager.Resource.Load<UI_GetPopup>("Prefabs/UI/Popup/UI_GetPopup");
    }

    public override void Interact()
    {
        Manager.Game.Player.StateMachine.ChangeState(PlayerState.Idle);
        if (_isGet)
            return;
        Inventory playerInventory = Manager.Game.Player.GetComponent<Inventory>();
        UI_GetPopup makePopup = Manager.UI.ShowPopUpUI(_getPopup);
        for (int i=0;i< _existingItems.Count;i++)
        {
            _existingItems[i].item.SetData();
            playerInventory.AddItem(_existingItems[i].item.Data, _existingItems[i].count);
            makePopup.UpdateItemList(_existingItems[i].item.Data, _existingItems[i].count);
            Debug.Log(_existingItems[i].item.Data.name + "È¹µæ!");
        }
        _isGet = true;
    }

    private void RandomItemAdd(int minCount,int maxCount)
    {
        int count = Random.Range(minCount, maxCount);

        for(int i=0;i<count;i++)
        {
            int randomItemIndex = Random.Range(0, _randomItem.Length);
            _existingItems.Add(new ItemEntity(_randomItem[randomItemIndex],1));
        }
    }

}
