using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
    public float baseDamage = 10; // ดาเมจของดาบ
    public float damageVariation = 0.2f;
    public void setDamage(float damage)
    {
        baseDamage += damage;
    }
    
    public float getDamage()
    {
        return baseDamage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            int finalDamage = CalculateDamage();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamge(finalDamage);
            }
        }
    }
    int CalculateDamage()
    {
        float randomFactor = Random.Range(1f - damageVariation, 1f + damageVariation);
        int finalDamage = Mathf.RoundToInt(baseDamage * randomFactor);
        return finalDamage;
        
    }
}
