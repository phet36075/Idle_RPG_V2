using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWeapon : MonoBehaviour
{
    public float baseDamage = 10; // ดาเมจของดาบ
    public float damageVariation = 0.2f;
    
    void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าชนกับผู้เล่น
        if (other.CompareTag("Player"))
        {
            // เข้าถึงสคริปต์ PlayerHealth ของผู้เล่น
            PlayerStats playerHealth = other.GetComponent<PlayerStats>();
            int finalDamage = CalculateDamage();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(finalDamage);  // ส่งดาเมจให้กับผู้เล่น
            }
        }
        
    }
    
    int CalculateDamage()
    {
        float randomFactor = Random.Range(1f - damageVariation, 1f + damageVariation);
        int finalDamage = Mathf.RoundToInt(baseDamage * randomFactor);
        return finalDamage;
        
    }

    private void Start()
    {
        
    }
}
