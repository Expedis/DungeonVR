﻿using UnityEngine;

[CreateAssetMenu(menuName = "Melee Weapon Data")]
public class MeleeWeaponData : ScriptableObject
{
    public enum WeaponType
    {
        Sword,
        Axe,
    };

    public string weaponName;
    public WeaponType weaponType;
    public GameObject weaponModel;
    public float weaponDamage;
    public float weaponCriticalChance;
    public bool canKnockBack;
    public float goldCost;
}