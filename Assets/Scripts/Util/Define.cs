using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int GET_COIN_AMOUNT = 10;
    public const int JUMP_MAX_COUNT = 1;
    public const float PLAY_TIME = 50 * 60; //50Ка
    public const int MAX_ZOMBIE = 100;
    public const int ZOMBIE_POOL_SIZE = 15;
    public const int SAVE_TIME = 5*60;
    public enum Scene
    {
        TItle,
        Game,
        Ending
    }

    public enum PlayerState
    {
        Idle,
        Interact,
        Die,
    }

    public enum ZombieState
    {

    }

    public enum PopupUI
    {
        Inventory,
        Shop
    }

    public enum StructureType
    {
        cottage,
        church,
        apartment,
        villa,
        school,
        mall,
        hospital
    }

    public enum Status
    {
        Hunger,
        Thirst,
        Fatigue,
        Stamina
    }


    public enum EndingType
    {
        InsatiableHunger,
        Breakthrough,
        GroundZero,
        Hope,
        HopeFromDespair,
        DespairFromHope
    }
}
