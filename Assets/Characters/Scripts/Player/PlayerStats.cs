using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public PlayerData PlayerData;
    // Base stats
    /*public float maxHealth = 100f;
    public float currentHealth;
    public float baseDamage = 40f;
    public float criticalChance = 0.05f;
    public float defense = 5f;
    public float armorPenetration = 0f;

    public bool isCritical;
    // Damage variation
    public float damageVariation = 0.2f; // 20% variation
*/
    
    public float currentHealth;
    private DamageDisplay _damageDisplay;
    
    
    private Animator animator;
    public AIController _AIController;

    public CharacterHitEffect hitEffect;
    void Start()
    {
        //hitEffect = GetComponent<CharacterHitEffect>();
       currentHealth = PlayerData.currentHealth;
        _damageDisplay = FindObjectOfType<DamageDisplay>();
    }
    public float CalculateFinalDamage(float incomingDamage, float targetDefense)
    {
        // Apply damage variation
        float variationMultiplier = Random.Range(1 - PlayerData.damageVariation, 1 + PlayerData.damageVariation);
        incomingDamage *= variationMultiplier;

        // Apply critical hit
        if (Random.value < PlayerData.criticalChance)
        {
            PlayerData.isCritical = true;
            incomingDamage *= 2f; // Double damage for critical hit
        }
        else
        {
            PlayerData.isCritical = false;
        }
        // Apply armor penetration
        float effectiveDefense = Mathf.Max(0, targetDefense - PlayerData.armorPenetration);

        // Calculate damage reduction
        float damageReduction = effectiveDefense / (effectiveDefense + 100f);

        // Calculate final damage
        float finalDamage = incomingDamage * (1f - damageReduction);

        return finalDamage;
    }
    public float CalculatePlayerAttackDamage(float skillDamageMultiplier = 1f)
    {
        // Start with base damage
        float attackDamage = PlayerData.baseDamage * skillDamageMultiplier;

        // Apply damage variation
        float variationMultiplier = Random.Range(1 - PlayerData.damageVariation, 1 + PlayerData.damageVariation);
        attackDamage *= variationMultiplier;

        // Apply critical hit
        PlayerData.isCritical = Random.value < PlayerData.criticalChance;
        if (PlayerData.isCritical)
        {
            attackDamage *= 2f; // Double damage for critical hit
            Debug.Log("Critical Hit!");
        }
       

        Debug.Log($"Attack Damage: {attackDamage} (Base: {PlayerData.baseDamage}, Skill Multiplier: {skillDamageMultiplier}, Critical: {PlayerData.isCritical})");
        return attackDamage;
    }
    public void TakeDamage(float damage)
    {
        //_AIController.FindNearestEnemy();
        hitEffect.StartHitEffect();
        
        float finalDamage = CalculateFinalDamage(damage, PlayerData.defense);
        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0f); // Ensure health doesn't go below 0

        Debug.Log($"Player took {finalDamage} damage. Current health: {currentHealth}");
        
        if (PlayerData.isCritical == true)
        {
            
        }
        else
        {
            _damageDisplay.DisplayDamage(finalDamage);
        }
        
        if (currentHealth <= 0)
        {
           
        }
        
        
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
       // _txtPlayerHealth.text = playerHealth.ToString();
    }
}
