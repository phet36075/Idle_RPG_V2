using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour
{
    public PlayerData PlayerData;
   
    public float currentHealth;
    public DamageDisplay _damageDisplay;
    public Animator animator;
    public GameObject regenEffect;
    public GameObject hitVFX;
    public Transform spawnVFXPosition;
    public Transform spawnRegenPosition;
    public bool isCritical;
    public CharacterHitEffect hitEffect;
    public string Stagename;
    private PlayerController _playerController;
    private AIController _aiController;
    private AllyRangedCombat _allyRangedCombat;
    public GameObject gameOverUI;
    //public float regenRate = 1f;
    
    public float regenInterval = 5f;
    public AudioManager _audioManager;

    public bool IsPlayerDie = false;
    void Start()
    {
        StartCoroutine(RegenerateHP());
        _allyRangedCombat = FindObjectOfType<AllyRangedCombat>();
        _playerController = FindObjectOfType<PlayerController>();
        //hitEffect = GetComponent<CharacterHitEffect>();
       currentHealth = PlayerData.currentHealth;
      //  _damageDisplay = FindObjectOfType<DamageDisplay>();
        _aiController = FindObjectOfType<AIController>();
       // _audioManager = FindObjectOfType<AudioManager>();
    }
    private IEnumerator RegenerateHP()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (currentHealth < PlayerData.maxHealth)
            {
                Quaternion rotation = Quaternion.Euler(-90f, 0, 0f);
                GameObject regenEfx = Instantiate(regenEffect, spawnRegenPosition.position,rotation );
                Destroy(regenEfx, 1f);
                
                currentHealth += PlayerData.regenRate;
                currentHealth = Mathf.Min(currentHealth, PlayerData.maxHealth);
                Debug.Log("Current HP: " + currentHealth);
            }
        }
    }
    
    public float CalculatePlayerAttackDamage(float skillDamageMultiplier = 1f)
    {
        // Start with base damage
        float attackDamage = (PlayerData.baseDamage + PlayerData.weaponDamage) * skillDamageMultiplier;
        
        // Apply damage variation
        float variationMultiplier = Random.Range(1 - PlayerData.damageVariation, 1 + PlayerData.damageVariation);
        attackDamage *= variationMultiplier;

        // Apply critical hit
        isCritical = Random.value < PlayerData.criticalChance;
        if (isCritical)
        {
            attackDamage *= 2f; // Double damage for critical hit
            Debug.Log("Critical Hit!");
        }
        
        Debug.Log($"Attack Damage: {attackDamage} (Base: {PlayerData.baseDamage}, Skill Multiplier: {skillDamageMultiplier}, Critical: {isCritical})");
        return attackDamage;
    }
    public void TakeDamage(float incomingDamage , float attackerArmorPenetration )
    {
        hitEffect.StartHitEffect();
        
       // Apply armor penetration
       float effectiveDefense = Mathf.Max(0, PlayerData.defense - attackerArmorPenetration);

       // Calculate damage reduction
       float damageReduction = effectiveDefense / (effectiveDefense + 100f);

       // Calculate final damage
       float finalDamage = incomingDamage * (1f - damageReduction);
       
        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0

        Debug.Log($"Player took {finalDamage} damage. Current health: {currentHealth}");
        
        _audioManager.PlayHitSound();
            _damageDisplay.DisplayDamage(finalDamage);
           Quaternion rotation = Quaternion.Euler(-90f, 0, 0f);
            GameObject effect = Instantiate(hitVFX, spawnVFXPosition.position,rotation );
            Destroy(effect, 1f);
            
        if (currentHealth <= 0)
        {
           Die();
        }
        
        
    }

    public void Heal(float amount)
    {
        currentHealth +=amount;
        if (currentHealth >= PlayerData.maxHealth)
        {
            currentHealth = PlayerData.maxHealth;
        }
    }

    public void ChangeWeapon(float weaponDmg)
    {
        PlayerData.weaponDamage = weaponDmg;
    }
    void Die()
    {
        IsPlayerDie = true;
       // currentHealth = PlayerData.maxHealth;
       animator.SetTrigger("Die");
       _allyRangedCombat.Die();
       _aiController.enabled = false;
       GetComponent<CapsuleCollider>().enabled = false;
       GetComponent<CharacterController>().enabled = false;
       _playerController.isAIActive = false;
       _playerController.enabled = false;
       gameOverUI.gameObject.SetActive(true);
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
