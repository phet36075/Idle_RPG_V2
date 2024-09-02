using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Skill1 : MonoBehaviour,ISkill
{
    public Animator animator;
    public float damageRadius = 10f;
    public float damageAmount = 10f;
    public float baseDamage = 10;
    public float cooldownTime = 5f;
    public GameObject skillEffectPrefab;
    public float damageLength = 5f;
    
    public GameObject skillEffectPrefabSlash;
   
    public float damageVariation = 0.2f;
    private bool isOnCooldown = false;
    public float maxAngle = 45f;
    public float skillRange = 2f; // ระยะห่างจากตัวละครที่สกิลจะเกิดผล
    public PlayerWeapon PlayerWeapon;
    private float lastUseTime = -Mathf.Infinity;
    private AIController _aiController;

    private void Start()
    {
        _aiController = FindObjectOfType<AIController>();
    }

    public void UseSkill()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            StartCoroutine(SkillSequence());
        }
    }
    int CalculateDamage()
    {
        float randomFactor = Random.Range(1f - damageVariation, 1f + damageVariation);
        int finalDamage = Mathf.RoundToInt(PlayerWeapon.baseDamage * randomFactor);
        return finalDamage;
    }
    IEnumerator SkillSequence()
    {
        // เล่น animation
        animator.SetTrigger("UseSkill");
        lastUseTime = Time.time;
        // รอให้ animation เล่นจบ (สมมติว่าใช้เวลา 1 วินาที)
        yield return new WaitForSeconds(2.8f);
        
        
        // คำนวณตำแหน่งและทิศทางสำหรับ effect
       Vector3 effectPosition = transform.position + transform.forward * 2f;
        Quaternion effectRotation = transform.rotation;
        
        int finalDamage = CalculateDamage();
        // ปล่อยดาเมจ
        Vector3 skillCenter = transform.position + transform.forward * skillRange;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * 2f, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (IsInFront(hitCollider.transform.position))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamge(finalDamage);
                }
            }
        }

        // แสดง effect
       
        GameObject effect = Instantiate(skillEffectPrefab, effectPosition, effectRotation);
        Destroy(effect, 2f); // ลบ effect หลังจาก 2 วินาที
        //_aiController.GetComponent<NavMeshAgent>().enabled = true;
        // เริ่มคูลดาวน์
        StartCoroutine(Cooldown());
    }
    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
    public void ShowEffect1()
    {
        Vector3 effectPosition = transform.position + transform.forward * 2f;
        Quaternion effectRotation = transform.rotation;
        GameObject effect = Instantiate(skillEffectPrefabSlash, effectPosition, effectRotation);
        Destroy(effect, 0.2f);
    }

    public void DoAnimDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * 2f, 2);
        foreach (var hitCollider in hitColliders)
        {
            int finalDamage = CalculateDamage();
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamge(finalDamage);
            }
            
        }
    }

    public void EnableNav()
    {
        _aiController.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void DisableNav()
    {
        _aiController.GetComponent<NavMeshAgent>().enabled = false;
    }
    public float GetCooldownPercentage()
    {
        float timeSinceLastUse = Time.time - lastUseTime;
        if (timeSinceLastUse < cooldownTime)
        {
            return 1f - (timeSinceLastUse / cooldownTime);
        }
        return 0f;
    }
    public float GetRemainingCooldownTime()
    {
        float timeSinceLastUse = Time.time - lastUseTime;
        if (timeSinceLastUse < cooldownTime)
        {
            return cooldownTime - timeSinceLastUse;
        }
        return 0f;
    }
   
    
    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownTime()
    {
        return cooldownTime;
    }
    bool IsInFront(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // ไม่สนใจความสูง

        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle <= maxAngle;
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        
        Vector3 capsuleStart = transform.position + transform.up * damageRadius;
        Vector3 capsuleEnd = capsuleStart + transform.forward * damageLength;
        
        UnityEditor.Handles.DrawWireDisc(capsuleStart, transform.forward, damageRadius);
        UnityEditor.Handles.DrawWireDisc(capsuleEnd, transform.forward, damageRadius);
        
        Gizmos.DrawLine(capsuleStart + transform.up * damageRadius, capsuleEnd + transform.up * damageRadius);
        Gizmos.DrawLine(capsuleStart - transform.up * damageRadius, capsuleEnd - transform.up * damageRadius);
        Gizmos.DrawLine(capsuleStart + transform.right * damageRadius, capsuleEnd + transform.right * damageRadius);
        Gizmos.DrawLine(capsuleStart - transform.right * damageRadius, capsuleEnd - transform.right * damageRadius);

        // วาดเส้นแสดงมุมการโจมตี
        Vector3 leftDirection = Quaternion.Euler(0, -maxAngle, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, maxAngle, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * damageLength);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * damageLength);
#endif
    }
    
   
}

