using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill2 : MonoBehaviour,ISkill
{
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
    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerWeapon = FindObjectOfType<PlayerWeapon>();
        _aiController = FindObjectOfType<AIController>();
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
        yield return new WaitForSeconds(0.1f);

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
        int finalDamage = CalculateDamage();
        // สร้างดาเมจรอบๆ ตัว
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamge(finalDamage);
            }
            
        }
        // แสดง effect
        GameObject effect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 2f); // ลบ effect หลังจาก 2 วินาที
        //Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(1f);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamge(finalDamage);
            }
            
        }
        _aiController.GetComponent<NavMeshAgent>().enabled = true;
        // สร้างดาเมจรอบๆ ตัว
      //  Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
       
        
        // รอให้ cooldown หมด
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
    int CalculateDamage()
    {
        float randomFactor = Random.Range(1f - damageVariation, 1f + damageVariation);
        int finalDamage = Mathf.RoundToInt(PlayerWeapon.baseDamage * randomFactor);
        return finalDamage;
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
}
