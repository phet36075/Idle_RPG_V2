using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Game/Player Data")]

public class PlayerData : ScriptableObject
{
   
    public float maxHealth = 100f;
    public float baseDamage = 40f;
    public float criticalChance = 0.05f;
    public float defense = 5f;
    public float armorPenetration = 0f;
    public bool isCritical;
    public float damageVariation = 0.2f; // 20% variation
    
}
