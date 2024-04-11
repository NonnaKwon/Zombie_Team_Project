using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int JUMP_MAX_COUNT = 1;
    public const float PLAY_TIME = 50 * 60; //50Ка
    public enum Scene
    {
        TItle,
        Game
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

}
