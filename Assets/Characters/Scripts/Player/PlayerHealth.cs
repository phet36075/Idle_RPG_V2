using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    public float maxHealth = 50;
    private Animator animator;

    
    /*void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าคุณชนกับดาบหรือไม่
        if (other.CompareTag("EnemyWeapon"))
            
        {
            EnemyAttack enemyAttack = this.GetComponent<EnemyAttack>();
            TakeDamage(10);  // ผู้เล่นจะเสียเลือดเมื่อโดนดาบ
        }
    }*/
    
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log("Player has been hit");
        if (playerHealth > 0)
        {
            Debug.Log("HP = : " + playerHealth);
        }

        if (playerHealth <= 0 )
        {
            Debug.Log("YOU DIE");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
