using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    
    public GameObject enemyPrefab;  // ตัว prefab ของศัตรู
    public float spawnInterval = 2f;  // ระยะเวลาระหว่างการเกิดของศัตรูแต่ละตัว
    public float spawnRadius = 10f;  // รัศมีของพื้นที่ที่ศัตรูจะเกิด
    public int maxEnemies = 10;  // จำนวนศัตรูสูงสุดที่จะเกิด
    public List<Transform> spawnPoints;  // ลิสต์ของตำแหน่งที่จะให้ศัตรูเกิด
    
    private int enemiesSpawned = 0;  // จำนวนศัตรูที่เกิดแล้ว
    private int enemiesDefeated = 0;  // จำนวนศัตรูที่ถูกกำจัดแล้ว

    public GameObject PortalEffect;
    void Start()
    {
        // เริ่มการเรียกฟังก์ชัน SpawnEnemy ซ้ำๆ ทุก spawnInterval วินาที
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // ตรวจสอบว่าจำนวนศัตรูที่เกิดแล้วยังไม่เกินค่าสูงสุด
        if (enemiesSpawned < maxEnemies)
        {
            Vector3 spawnPos;

            if (spawnPoints.Count > 0)
            {
                // สุ่มเลือกตำแหน่งจาก spawnPoints
                spawnPos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            }
            else
            {
                // ถ้าไม่มี spawnPoints ให้สุ่มตำแหน่งภายในวงกลมเหมือนเดิม
                Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
                spawnPos = new Vector3(randomPos.x, 0, randomPos.y) + transform.position;
            }

           
            // สร้างศัตรูที่ตำแหน่งที่เลือกได้
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
          
            //Destroy(SpawnFX,0.5f);
            // เพิ่มจำนวนศัตรูที่เกิดแล้ว
            enemiesSpawned++;

            // เพิ่ม EnemyDefeatedNotifier component ให้กับศัตรู
            enemy.AddComponent<EnemyDefeatedNotifier>().spawner = this;
        }
        else
        {
            // หยุดการ spawn เมื่อครบจำนวนที่กำหนด
            CancelInvoke("SpawnEnemy");
        }
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        
        // ตรวจสอบว่าได้กำจัดศัตรูครบตามจำนวนที่กำหนดหรือยัง
        if (enemiesDefeated >= maxEnemies)
        {
            Debug.Log("All enemies defeated!");
            PortalEffect.gameObject.SetActive(true);
            // ทำอะไรก็ตามที่คุณต้องการเมื่อกำจัดศัตรูครบจำนวน เช่น แสดงข้อความชนะ หรือโหลดด่านต่อไป
        }
    }
}

// Component เพิ่มเติมสำหรับแจ้งเตือนเมื่อศัตรูถูกกำจัด
public class EnemyDefeatedNotifier : MonoBehaviour
{
    public EnemySpawner spawner;

    void OnDestroy()
    {
        // เมื่อ GameObject นี้ถูกทำลาย (ศัตรูถูกกำจัด) ให้แจ้ง EnemySpawner
        spawner.EnemyDefeated();
    }
   
}
