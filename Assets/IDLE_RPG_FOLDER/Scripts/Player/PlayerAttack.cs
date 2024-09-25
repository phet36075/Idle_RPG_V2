using System;
using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    
    public AllyRangedCombat rangedAllies;  // เพิ่มอาร์เรย์ของพวกพ้องที่โจมตีระยะไกล
    
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboCooldown = 1f;
    public bool isAttacking = false;
    private Transform vfxPos;
    public GameObject attackVFX;
    public Trail _Trail;
    
    public float detectionRadius = 5f; // รัศมีในการตรวจจับศัตรู
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่
    public float attackRange = 2f; // ระยะโจมตี
    public float attackRadius = 1f; // รัศมีการโจมตี
    private Transform nearestEnemy; // เก็บ Transform ของศัตรูที่ใกล้ที่สุด
    private bool isMovingToEnemy = false; // เพิ่มตัวแปรเพื่อตรวจสอบว่ากำลังเคลื่อนที่หาศัตรูหรือไม่
    
    private float currentSpeed = 0f; // เพิ่มตัวแปรเก็บความเร็วปัจจุบัน
    
    public Transform attackPoint; // จุดศูนย์กลางของการโจมตี
    public LayerMask enemyLayers; // Layer ของศัตรู
    void Start()
    {
        
       
    }

    void Update()
    {
        if (Time.time - lastAttackTime > comboCooldown)
        {
            comboStep = 0;
            isAttacking = false;
            isMovingToEnemy = false;
        }
        
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Attack();
        }

        if (isMovingToEnemy && nearestEnemy != null)
        {
            MoveTowardsEnemy();
        }
        else
        {
            StopMoving();
        }

        // อัพเดท animator ด้วยค่า Speed ปัจจุบัน
       
    }
    public void Attack()
    {
        if (!isAttacking)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
            comboStep++;
            rangedAllies.CallAlliesToAttack();

            FindNearestEnemy();

            if (nearestEnemy == null)
            {
                PerformAttackAnimation();
            }
            else
            {
                float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.position);
                if (distanceToEnemy <= attackRange)
                {
                    PerformAttackAnimation();
                }
                else
                {
                    isMovingToEnemy = true;
                    MoveTowardsEnemy();
                }
            }
        }
    }
    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        nearestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hitCollider.transform;
                }
            }
        }
    }

    private void MoveTowardsEnemy()
    {
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.position);
            animator.SetFloat("Speed", currentSpeed);
            if (distanceToEnemy > attackRange)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(nearestEnemy);
                
                // เริ่มเล่น animation การเดิน
                currentSpeed = moveSpeed;
            }
            else
            {
                isMovingToEnemy = false;
                StopMoving();
                PerformAttackAnimation();
            }
        }
        else
        {
            isMovingToEnemy = false;
            StopMoving();
        }
    }

    private void StopMoving()
    {
        // หยุดเล่น animation การเดิน
        currentSpeed = 0f;
    }

    private void PerformAttackAnimation()
    {
        // หยุดเล่น animation การเดินก่อนเริ่ม animation การโจมตี
        StopMoving();

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
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            
        }
    }
    
    public void PerformAttack()
    {
        attackVFX.SetActive(true);
        _Trail.enabled = true;
        float effectDuration = 0.2f;
        Invoke("StopEffect", effectDuration);

        // ตรวจสอบและสร้างความเสียหายให้กับศัตรูที่อยู่ในระยะโจมตี
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                PlayerManager playerManager = GetComponent<PlayerManager>();
                float attackDamage = playerManager.CalculatePlayerAttackDamage();
                enemyHealth.TakeDamage(attackDamage, playerManager.PlayerData.armorPenetration);
            }
        }
    }
    
    private void StopEffect()
    {
        attackVFX.SetActive(false);
        _Trail.enabled = false;
    }
    
    public void EndAttack()
    {
        isAttacking = false;
        isMovingToEnemy = false;
        nearestEnemy = null;
        StopMoving();
    }
    
    public void StartAttack()
    {
        isAttacking = true;
    }

   
    
    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                PlayerManager playerManager = GetComponent<PlayerManager>();
                float attackDamage = playerManager.CalculatePlayerAttackDamage();
                enemyHealth.TakeDamage(attackDamage, playerManager.PlayerData.armorPenetration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (nearestEnemy != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nearestEnemy.position);
        }
    }
    
}
