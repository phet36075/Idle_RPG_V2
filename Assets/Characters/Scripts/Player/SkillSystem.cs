using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        StartCoroutine(SkillSequence());
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

        // เริ่มคูลดาวน์
        StartCoroutine(Cooldown());
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
    bool IsInFront(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // ไม่สนใจความสูง

        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle <= maxAngle;
    }
    IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
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

