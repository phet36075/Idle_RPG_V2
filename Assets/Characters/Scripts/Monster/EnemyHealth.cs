using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
    [Header("-----------Health----------")]
    public float maxhealth = 50f;
    public string enemyName = "Enemy";
    public float currentHealth;
    public Slider healthBar;
    public bool isDead = false;
    
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

   void Start()
   {
       currentHealth = maxhealth;
       healthBar.maxValue = maxhealth;
       healthBar.value = currentHealth;
       spawner = FindObjectOfType<EnemySpawner>();
       
   }
   
    public void TakeDamge(float damage)
    {
       if (!isHurt)
       {
           CharacterHitEffect.StartHitEffect();
           // ลดเลือดหรือค่าพลังของศัตรูตามการโจมตี
           animator.SetBool("isHurt", true);
           isHurt = true;
           Invoke("ResetHurt", 0.5f);
       }
       
       
            currentHealth -= damage;
            UpdateHealthBar();
            DamageDisplay damageDisplay = this.GetComponent<DamageDisplay>();
            damageDisplay.DisplayDamage(damage);
            if (currentHealth > 0)
            {
                    Stagger();
            }
            if (currentHealth <= 0)
            {
               
                Die();
            }
           
     
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
    
    
    void Die()
    {
    
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject,3f);
        enemyCollider.enabled = false;
        animator.SetBool("IsWalking",false);
        animator.SetBool("IsAttacking",false);
        currentHealth = 0;
    }
    
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
