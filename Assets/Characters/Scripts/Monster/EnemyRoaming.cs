using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRoaming : MonoBehaviour
{
    public float roamRadius = 10f;

    public float roamDelay = 3f;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        RoamToNewPosition();
        animator = GetComponent<Animator>();
    }

    void RoamToNewPosition()
    {
        Debug.Log("Roaming");
        // คำนวณตำแหน่งใหม่แบบสุ่มภายในรัศมี
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startingPosition;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, roamRadius, -1))
        {
//            Debug.Log("Moving to new position: " + navHit.position);  // ตรวจสอบตำแหน่งเป้าหมาย
            navMeshAgent.SetDestination(navHit.position);
        }
        else
        {
     //       Debug.Log("No valid NavMesh position found.");
        }
        // ตั้งเวลาเพื่อเรียกการ Roam ใหม่หลังจากถึงจุดหมาย
        Invoke("RoamToNewPosition", roamDelay + Random.Range(0f, 3f));
    }
    

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("IsWalking", true);  // เริ่มเล่นอนิเมชั่นเดิน
        }
        else
        {
            animator.SetBool("IsWalking", false); // หยุดเล่นอนิเมชั่นเดิน
        }
    }
}
