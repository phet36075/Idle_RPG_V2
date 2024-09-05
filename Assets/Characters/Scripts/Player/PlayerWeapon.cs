using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
  
    private PlayerAttack _playerAttack;
    private PlayerStats _playerStats;
    private void Start()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _playerStats = FindObjectOfType<PlayerStats>();
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
               // _playerAttack.PerformAttack();
               
               PlayerStats playerStats = GetComponent<PlayerStats>();
              
                
               //Skill Damage
               float attackDamage = playerStats.CalculatePlayerAttackDamage();
               enemyHealth.TakeDamage(attackDamage,playerStats.PlayerData.armorPenetration);
            }
        }
    }
   
}
