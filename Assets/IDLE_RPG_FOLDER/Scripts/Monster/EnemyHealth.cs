using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
    [Header("-----------Health----------")]
    public EnemyData EnemyData;
    public Transform spawnVFXPosition;
    public float currentHealth;
 /*   public float maxhealth = 50f;
    public string enemyName = "Enemy";
    public float defense = 5f;*/
  
  
    public Slider healthBar;
    public bool isDead = false;

    public GameObject hitVFX;
    [Header("----------Animator----------")]
    public Animator animator;
   
    [Header("----------Enemy Collider----------")]
    
    public Collider enemyCollider;
    private EnemySpawner spawner;
    
    [Header("----------Stun duration----------")]
    public float staggerDuration = 0.5f;  // ระยะเวลาที่ศัตรูหยุดชะงัก
    private float lastTimeStagger = 0;
    public float cooldownStagger = 4;
   private bool isHurt;
   public CharacterHitEffect CharacterHitEffect;
   private PlayerManager _playerManager;
   private DamageDisplay _damageDisplay;
   void Start()
   {
       currentHealth = EnemyData.maxhealth;
       healthBar.maxValue = EnemyData.maxhealth;
       healthBar.value = currentHealth;
       spawner = FindObjectOfType<EnemySpawner>();
       _playerManager = FindObjectOfType<PlayerManager>();
       _damageDisplay = FindObjectOfType<DamageDisplay>();
   }

   public void TakeDamage(float incomingDamage)
   {
       if (!isHurt)
       {
           currentHealth -= incomingDamage;
           currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0
           
           CharacterHitEffect.StartHitEffect();
           animator.SetBool("isHurt", true);
           isHurt = true;
           Invoke("ResetHurt", 0.5f);
       }
           
       UpdateHealthBar();

       if (_playerManager.isCritical == true)
       {
           _damageDisplay.DisplayDamageCritical(incomingDamage);
           _playerManager.isCritical = false;
       }
       else
       {
           _damageDisplay.DisplayDamage(incomingDamage);
       }
       
       if (currentHealth > 0)
       {
           Stagger();
       }
       if (currentHealth <= 0)
       {
           Die();
       }
   }

   public void TakeDamage(float incomingDamage, float attackerArmorPenetration)
    {
        // Apply armor penetration
        float effectiveDefense = Mathf.Max(0, EnemyData.defense - attackerArmorPenetration);

        // Calculate damage reduction
        float damageReduction = effectiveDefense / (effectiveDefense + 100f);

        // Calculate final damage
        float finalDamage = incomingDamage * (1f - damageReduction);
        
       if (!isHurt)
       {
          
           currentHealth -= finalDamage;
           currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0
           
           CharacterHitEffect.StartHitEffect();
           animator.SetBool("isHurt", true);
           isHurt = true;
           Invoke("ResetHurt", 0.5f);
       }
           
       GameObject effect = Instantiate(hitVFX, spawnVFXPosition.position, spawnVFXPosition.rotation);
       Destroy(effect, 1f);
            UpdateHealthBar();

            if (_playerManager.isCritical == true)
            {
                _damageDisplay.DisplayDamageCritical(finalDamage);
                _playerManager.isCritical = false;
            }
            else
            {
                _damageDisplay.DisplayDamage(finalDamage);
            }
            
            
            if (currentHealth > 0)
            {
                    Stagger();
            }
            if (currentHealth <= 0)
            {
                Die();
            }
            
            
            
    }
    public void TakeDamage(DamageInfo damageInfo)
    {
        float finalDamage = CalculateDamage(damageInfo);
        currentHealth -= finalDamage;

        // แสดง effect ตามประเภทดาเมจ
        ShowDamageEffect(damageInfo);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private float CalculateDamage(DamageInfo damageInfo)
    {
        // คำนวณดาเมจตามประเภท, ความต้านทาน, คริติคอล ฯลฯ
        return damageInfo.amount;
    }

    private void ShowDamageEffect(DamageInfo damageInfo)
    {
        // แสดง particle effect หรือเสียงตามประเภทดาเมจ
    }

    private void Die()
    {
        CurrencyManager.Instance.AddMoney(100);
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject,3f);
        enemyCollider.enabled = false;
        animator.SetBool("IsWalking",false);
        animator.SetBool("IsAttacking",false);
        currentHealth = 0;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return EnemyData.maxhealth;
    }
    
    
    
    
    
    
    
    
    
    public void ResetHurt()
    {
        animator.SetBool("isHurt", false);
        isHurt = false;
    }
    public void OnHurtAnimationEnd()
    {
        ResetHurt();
    }
    void Stagger()
    {
       
        if (animator != null && Time.time > lastTimeStagger  )
        {
            lastTimeStagger = Time.time + cooldownStagger;
            animator.SetTrigger("Hit");
        }
        
        // หยุดการเคลื่อนไหวของศัตรูชั่วคราว
        GetComponent<EnemyAttack>().enabled = false;  // ปิดการเคลื่อนไหว
        Invoke("RecoverFromStagger", staggerDuration);  // กำหนดเวลาหยุดชะงัก
        
    }
    void RecoverFromStagger()
    {
    
        GetComponent<EnemyAttack>().enabled = true;  // เปิดการเคลื่อนไหวกลับมา
    }
    
    
    /*void Die()
    {
    
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject,3f);
        enemyCollider.enabled = false;
        animator.SetBool("IsWalking",false);
        animator.SetBool("IsAttacking",false);
        currentHealth = 0;
    }*/
    
    void UpdateHealthBar()
    {
        StartCoroutine(SmoothHealthBar());
    }

    IEnumerator SmoothHealthBar()
    {
        float elapsedTime = 0f;
        float duration = 0.2f; // ระยะเวลาที่ต้องการให้การลดลงของแถบลื่นไหล
        float startValue = healthBar.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthBar.value = Mathf.Lerp(startValue, currentHealth, elapsedTime / duration);
            yield return null;
        }

        healthBar.value = currentHealth;
    }
    
    
    void Update()
    {
        
    }
}
