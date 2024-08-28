using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public float attackDamage = 10f;
    public Collider weaponCollider;
    
    public AllyRangedCombat[] rangedAllies;  // เพิ่มอาร์เรย์ของพวกพ้องที่โจมตีระยะไกล
    
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboCooldown = 1f;
    public bool isAttacking = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        weaponCollider.enabled = false;
    }
// Update is called once per frame
    void Update()
    {
        if (Time.time - lastAttackTime > comboCooldown)
        {
            comboStep = 0;
            isAttacking = false;
        }
        
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Attack();
            
        }
    }
    public void Attack()
    {
        //isAttacking = true;
        lastAttackTime = Time.time;
        isAttacking = true;
        comboStep++;
       
        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
            CallAlliesToAttack();
            
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (comboStep == 3)    
        {
            animator.SetTrigger("Attack3");
            comboStep = 0;
        }
    }
    void CallAlliesToAttack()
    {
        Transform enemy = FindEnemyInRange();  // ฟังก์ชันเพื่อค้นหาศัตรูในระยะใกล้
        if (enemy != null)
        {
            foreach (AllyRangedCombat ally in rangedAllies)
            {
                ally.AttackEnemy(enemy);
            }
        }
    }

    Transform FindEnemyInRange()
    {
        float detectionRadius = 10f; // ระยะการตรวจจับศัตรู
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy")) // ตรวจสอบว่าเป็นศัตรูหรือไม่
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider.transform;
                }
            }
        }

        return closestEnemy; // คืนค่า Transform ของศัตรูที่อยู่ใกล้ที่สุด
    }
    
    
    
    public void EndAttack()
    {
        isAttacking = false;
    }
    
    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit With Sword");
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamge(attackDamage);
            }
        }
    }*/

    
}
