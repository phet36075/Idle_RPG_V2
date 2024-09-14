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
    private DamageDisplay _damageDisplay;
    public Animator animator;
  

    public bool isCritical;
    public CharacterHitEffect hitEffect;
    public string Stagename;
    private PlayerController _playerController;
    private AIController _aiController;
    private AllyRangedCombat _allyRangedCombat;
    public GameObject gameOverUI;
    void Start()
    {
        _allyRangedCombat = FindObjectOfType<AllyRangedCombat>();
        _playerController = FindObjectOfType<PlayerController>();
        //hitEffect = GetComponent<CharacterHitEffect>();
       currentHealth = PlayerData.currentHealth;
        _damageDisplay = FindObjectOfType<DamageDisplay>();
        _aiController = FindObjectOfType<AIController>();
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
        
       
            _damageDisplay.DisplayDamage(finalDamage);
            
        if (currentHealth <= 0)
        {
           Die();
        }
        
        
    }

    void Die()
    {
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
