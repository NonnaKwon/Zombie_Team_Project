using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon")]
public class WeaponData : ItemData
{
    public float minDamage;
    public float maxDamage;
    public string animationLayer;
    public float attackRange;
    public float attackSpeed;
}

