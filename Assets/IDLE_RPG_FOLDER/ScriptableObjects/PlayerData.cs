using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Game/Player Data")]

public class PlayerData : ScriptableObject
{
    public float DefaultmaxHealth;
    public float maxHealth = 100f;
    public float currentHealth;
    public float baseDamage = 10f;
    public float DefaultbaseDamage;
    public float weaponDamage = 10f;
    public float criticalChance = 0.05f;
    public float defense = 5f;
    public float armorPenetration = 0f;
   // public bool isCritical;
    public float damageVariation = 0.2f; // 20% variation
    public float regenRate = 1f;
    public float criticalDamage = 2f;
    public int stage = 1;
    // ชั่วคราว
    public int WeaponupgradeCost = 100;
    public int Weaponlevel = 1;
    
    public int healthUpgradeCost = 100;
    public int healthLevel = 1;
    
    public int regenRateCost = 100;
    public int regenRateLevel = 1;
    
    public int criticalRateCost = 100;
    public int criticalRateLevel = 1;
    
    public int defenseCost = 100;
    public int defenseLevel = 1;

    public int armorPenetrationCost = 100;
    public int armorPenetrationLevel = 1;
    
    public int criticalDamageCost = 100;
    public int criticalDamageLevel = 1;
    private void OnEnable()
    {
       
        ResetToDefault();
    }
    
    public void ResetToDefault()
    {

       
        baseDamage = DefaultbaseDamage;
        weaponDamage = 5f;
        criticalChance = 0.05f;
        defense = 5f;
        armorPenetration = 0;
        stage = 1;
        
        Weaponlevel = 1;
        WeaponupgradeCost = 100;

        maxHealth = DefaultmaxHealth;
        currentHealth = maxHealth;
        healthLevel = 1;
        healthUpgradeCost = 100;

        regenRate = 5;
        regenRateLevel = 1;
        regenRateCost = 250;

        criticalRateLevel = 1;
        criticalRateCost = 1000;

        defense = 5;
        defenseLevel = 1;
        defenseCost = 125;

        armorPenetration = 0;
        armorPenetrationLevel = 1;
        armorPenetrationCost = 200;

        criticalDamage = 2;
        criticalDamageLevel = 1;
        criticalDamageCost = 2500;

    }
}
