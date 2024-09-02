using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour,ISkill
{
    public Animator animator;
    public float damageAmount = 20f;
    public float cooldownTime = 8f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private bool isOnCooldown = false;

    public void UseSkill()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            StartCoroutine(SkillSequence());
        }
    }

    private IEnumerator SkillSequence()
    {
        animator.SetTrigger("UseProjectileSkill");
        yield return new WaitForSeconds(0.5f);

        Vector3 spawnPosition = transform.position + transform.forward + transform.up;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownTime()
    {
        return cooldownTime;
    }
}
