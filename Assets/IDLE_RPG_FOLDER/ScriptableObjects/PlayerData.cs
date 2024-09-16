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
    public float weaponDamage = 10f;
    public float criticalChance = 0.05f;
    public float defense = 5f;
    public float armorPenetration = 0f;
   // public bool isCritical;
    public float damageVariation = 0.2f; // 20% variation
    public float regenRate = 1f;
    public int stage = 1;
    // ชั่วคราว
    public int upgradeCost = 100;
    public int level = 1;
    private void OnEnable()
    {
        currentHealth = maxHealth;
        ResetToDefault();
    }
    
    public void ResetToDefault()
    {
        maxHealth = 300f;
        baseDamage = 10f;
        weaponDamage = 5f;
        criticalChance = 0.05f;
        defense = 5f;
        armorPenetration = 0;
        stage = 1;
        level = 1;
        upgradeCost = 100;
    }
}
