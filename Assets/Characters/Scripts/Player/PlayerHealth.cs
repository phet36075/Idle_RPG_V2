using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    public float maxHealth = 1000;
    private Animator animator;
    public AIController _AIController;

    public CharacterHitEffect hitEffect;
    void Start()
    {
        //hitEffect = GetComponent<CharacterHitEffect>();
        playerHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        //_AIController.FindNearestEnemy();
        hitEffect.StartHitEffect();
        playerHealth -= damage;
        DamageDisplay damageDisplay = this.GetComponent<DamageDisplay>();
        damageDisplay.DisplayDamage(damage);
        
        Debug.Log("Player has been hit");
        if (playerHealth > 0)
        {
            Debug.Log("HP = : " + playerHealth);
        }

        if (playerHealth <= 0 )
        {
            Debug.Log("YOU DIE");
        }
        
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
       // _txtPlayerHealth.text = playerHealth.ToString();
    }
}
