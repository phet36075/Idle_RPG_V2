using System;
using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
  
    public Collider weaponCollider;
    
    public AllyRangedCombat rangedAllies;  // เพิ่มอาร์เรย์ของพวกพ้องที่โจมตีระยะไกล
    
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboCooldown = 1f;
    public bool isAttacking = false;
    private Transform vfxPos;
    public GameObject attackVFX;
    public Trail _Trail;

    
    void Start()
    {
        weaponCollider.enabled = false;
       
    }

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
        lastAttackTime = Time.time;
        isAttacking = true;
        comboStep++;
        rangedAllies.CallAlliesToAttack();
        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
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
    
    public void PerformAttack()
    {
        attackVFX.gameObject.SetActive(true);
        _Trail.enabled = true;
        float effectDuration = 0.2f;
       
        Invoke("StopEffect", effectDuration);
    }
    
    private void StopEffect()
    {
        attackVFX.gameObject.SetActive(false);
       _Trail.enabled = false;
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
    
    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                // _playerAttack.PerformAttack();
               
                PlayerManager playerManager = GetComponent<PlayerManager>();
              
                
                //Skill Damage
                float attackDamage = playerManager.CalculatePlayerAttackDamage();
                enemyHealth.TakeDamage(attackDamage,playerManager.PlayerData.armorPenetration);
            }
        }
    }
    
    
}
