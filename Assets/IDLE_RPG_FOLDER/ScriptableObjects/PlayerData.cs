using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Game/Player Data")]

public class PlayerData : ScriptableObject
{
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
        currentHealth = maxHealth;
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

        healthLevel = 1;
        healthUpgradeCost = 100;

        regenRateLevel = 1;
        regenRateCost = 250;

        criticalRateLevel = 1;
        criticalRateCost = 1000;

        defenseLevel = 1;
        defenseCost = 125;

        armorPenetrationLevel = 1;
        armorPenetrationCost = 200;

        criticalDamageLevel = 1;
        criticalDamageCost = 2500;

    }
}
