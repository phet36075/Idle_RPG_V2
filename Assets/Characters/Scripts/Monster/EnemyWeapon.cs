using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int damage = 10;  // ดาเมจของดาบ

    void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าชนกับผู้เล่น
        if (other.CompareTag("Player"))
        {
            // เข้าถึงสคริปต์ PlayerHealth ของผู้เล่น
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // ส่งดาเมจให้กับผู้เล่น
            }
        }
    }
}
