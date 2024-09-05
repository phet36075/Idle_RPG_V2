using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill2 : MonoBehaviour,ISkill
{
    [Header("-------------Damage-------------")]
    public float skill2DmgDashThrough = 0.6f;
    public float skill2DmgAfterDashThrough = 1.5f;
        
    public float cooldownTime = 10f;
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float damageRadius = 2f;
    public float damageAmount = 20f;
    public GameObject dashEffectPrefab;
    public string dashAnimationTrigger = "DashAttack";
    public float damageVariation = 0.2f;
    private Animator animator;
    private bool isOnCooldown = false;
    private float lastUseTime = -Mathf.Infinity;
    private PlayerWeapon PlayerWeapon;
    private AIController _aiController;
    public int numberOfHits = 5;
    public float timeBetweenHits = 0.1f;
    private float actualCooldownStartTime;
    private float skillDuration;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerWeapon = FindObjectOfType<PlayerWeapon>();
        _aiController = FindObjectOfType<AIController>();
        skillDuration = 3; 
    }

    public void UseSkill()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(DashAttackCoroutine());
        }
    }

    private IEnumerator DashAttackCoroutine()
    {
        isOnCooldown = true;
        lastUseTime = Time.time;

        // เล่นอนิเมชั่น
        animator.SetTrigger(dashAnimationTrigger);

        // รอให้อนิเมชั่นเริ่ม (ปรับตามความเหมาะสม)
        yield return new WaitForSeconds(1f);
        
        
       _aiController.GetComponent<NavMeshAgent>().enabled = false;
        // เริ่มการพุ่ง
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * dashDistance;
        float elapsedTime = 0f;
        
       
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ตำแหน่งสุดท้าย
        transform.position = endPosition;
        GameObject effect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 2f); // ลบ effect หลังจาก 2 วินาที
        for (int i = 0; i < numberOfHits; i++)
        {
            PerformSingleHit();
            yield return new WaitForSeconds(timeBetweenHits);
        }
        
        // แสดง effect
        
        
        yield return new WaitForSeconds(0.5f);
        Collider[] hitColliders2 = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders2)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                PlayerStats playerStats = GetComponent<PlayerStats>();
                float enemyDefense = 10f;
                
                
                //Skill Damage
                float skillDamage = playerStats.CalculatePlayerAttackDamage(skill2DmgAfterDashThrough);
                enemyHealth.TakeDamage(skillDamage,playerStats.PlayerData.armorPenetration);
            }
            
        }
        _aiController.GetComponent<NavMeshAgent>().enabled = true;
        // รอให้ cooldown หมด
        actualCooldownStartTime = Time.time;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    void PerformSingleHit()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                PlayerStats playerStats = GetComponent<PlayerStats>();
                float enemyDefense = 10f;
                
                
                float skillDamage = playerStats.CalculatePlayerAttackDamage(skill2DmgDashThrough);
                
                enemyHealth.TakeDamage(skillDamage,playerStats.PlayerData.armorPenetration);
               // damageable.TakeDamage(finalDamage);
            }
        }
        
    }
    
    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownTime()
    {
        return cooldownTime;
    }

    public float GetCooldownPercentage()
    {
        if (!isOnCooldown)
        {
            return 0f;
        }

        float totalCooldownTime = skillDuration + cooldownTime;
        float elapsedTime = Time.time - lastUseTime;
        return 1f - Mathf.Clamp01(elapsedTime / totalCooldownTime);
    }

    public float GetRemainingCooldownTime()
    {
        if (!isOnCooldown)
        {
            return 0f;
        }

        float timeSinceUse = Time.time - lastUseTime;
        if (timeSinceUse < skillDuration)
        {
            // สกิลยังทำงานอยู่
            return cooldownTime + (skillDuration - timeSinceUse);
        }
        else
        {
            // สกิลจบแล้ว กำลังอยู่ใน cooldown จริงๆ
            return cooldownTime - (Time.time - actualCooldownStartTime);
        }
    }
    
    private void OnDrawGizmos()
    {
       
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, damageRadius);
        
    }
    
    
}



