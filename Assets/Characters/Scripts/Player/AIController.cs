using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    public Transform target;  // ศัตรูเป้าหมาย
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    public float rotationSpeed = 5f;
    public GameObject indicatorPrefab; // Prefab ของ Indicator
    private GameObject currentIndicator;
    
    private Transform nextLevelTarget; 
    
    private bool moveToNextLevel = false;
    public StageManager _StageManager;
    public int stage = 0;
    public EnvironmentManager _EnvironmentManager;
    public EnemySpawner _EnemySpawner;
    public bool isLastStage;
    
    private void OnDisable()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }
    }
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
   public void FindNearestEnemy()
   {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();

            if (enemyScript != null && !enemyScript.isDead)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
           /* TargetIndicator targetIndicator = target.GetComponent<TargetIndicator>();
            targetIndicator.ShowIndicator();*/
           if (currentIndicator != null)
           {
               Destroy(currentIndicator);
           }
           // show indicator
           currentIndicator = Instantiate(indicatorPrefab, target.transform.position, Quaternion.identity);
           
            
        }
        else
        {
            target = null; // ไม่มีศัตรูในระยะ
           
          /*  TargetIndicator targetIndicator = target.GetComponent<TargetIndicator>();
            targetIndicator.HideIndicator();*/
            Destroy(currentIndicator);
            
            
        }
    }


    void Update()
    {
        if (moveToNextLevel)
        {
            MoveToNextLevel();
        }
        if (currentIndicator != null)
        {
            
            currentIndicator.transform.position = new Vector3(target.transform.position.x,currentIndicator.transform.position.y,target.transform.position.z);
            
        }
       /* if (target == null || Vector3.Distance(transform.position, target.position) > attackRange)
        {
            FindNearestEnemy();
        }
        FindNearestEnemy();
        
        if (!target) // หาเป้าหมายถ้าไม่มี
        {
            FindNearestEnemy();
        }*/
       if (target != null)
       {
           EnemyHealth targetEnemy = target.GetComponent<EnemyHealth>();
           if (targetEnemy != null && targetEnemy.isDead)
           {
               // หากศัตรูตายแล้ว หาศัตรูใหม่
                   FindNearestEnemy();
           }
           else
           {
               // ทำงานตามปกติ
               if (moveToNextLevel == false)
               {
                   agent.SetDestination(target.position);
               }
              
               
               float distanceToTarget = Vector3.Distance(transform.position, target.position);
               if (distanceToTarget <= attackRange)
               {
                   agent.isStopped = true;
                   RotateTowardsTarget();
                   if (Time.time >= nextAttackTime)
                   {
                       PlayerAttack playerAttack = this.GetComponent<PlayerAttack>();
                       playerAttack.Attack();
                       nextAttackTime = Time.time + attackCooldown;
                   }
               }
               else
               {
                   agent.isStopped = false;
               }

               animator.SetFloat("Speed", agent.velocity.magnitude);
           }
       }
       else
       {
           // หาศัตรูใหม่เมื่อไม่มีเป้าหมาย
           if (moveToNextLevel == false)
           {
               FindNearestEnemy();
               Destroy(currentIndicator);
           }
           
         //  FindNearestEnemy();
         
          
       }
    }
    
     public void MoveToNextLevel()
     {
        // _EnemySpawner.ClearEnemies();
         moveToNextLevel = true;
         Destroy(currentIndicator);
         nextLevelTarget = GameObject.FindGameObjectWithTag("NextLevelPosition").transform;
        agent.SetDestination(nextLevelTarget.position);
        
        _EnemySpawner.enemiesDefeated = 0;
        Debug.Log("กำลังเดินไปที่จุด!");
        // ตรวจสอบว่าผู้เล่นไปถึงจุดหมายปลายทางหรือไม่
        if (Vector3.Distance(transform.position, nextLevelTarget.position) < 0.5f)
        {
            if (_StageManager.stage != _EnvironmentManager.AllStage)
            {
                _EnemySpawner.enemiesDefeated = 0;
                _StageManager.stage += 1;
                Debug.Log("ถึงด่านใหม่แล้ว! ด่านที่ :" + stage);
                _EnvironmentManager.LoadEnvironment(_StageManager.stage);
                moveToNextLevel = false;
            }
            else
            {
                isLastStage = true;
                Debug.Log("ด่านสุดท้ายแล้ว ด่านที่ :" + stage);
                moveToNextLevel = false;
                FindNearestEnemy();
                agent.SetDestination(target.position);
            }
            
           
         //  _StageManager.GoToNextLevel();
           //moveToNextLevel = false;
           
           
            // ใส่ Logic การโหลดด่านใหม่ที่นี่ เช่น
            // SceneManager.LoadScene("NextLevel");
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    void Attack()
    {
        // เรียกใช้อนิเมชันการโจมตี
        
        animator.SetTrigger("Attack1");

        // คุณสามารถเพิ่มโค้ดสำหรับคำนวณความเสียหายและอื่น ๆ ที่นี่
    }
    
    void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        //Debug.Log("Direction: " + direction);  // ตรวจสอบทิศทางการหมุน
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //Debug.Log("Target Rotation: " + lookRotation.eulerAngles);  // ตรวจสอบการหมุนที่ควรจะเป็น
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    
    
}
