using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab ของศัตรู
    public GameObject bossPrefab;
    public Transform[] spawnPoints; // จุดที่จะใช้สุ่มเกิดศัตรู
    public int maxEnemies = 10; // จำนวนศัตรูสูงสุดที่สามารถเกิดได้พร้อมกัน
    public int totalEnemiesToDefeat = 20; // จำนวนศัตรูทั้งหมดที่ต้องกำจัดเพื่อเปิดด่านใหม่
    public float spawnInterval = 2f; // เวลาระหว่างการเกิดแต่ละตัว
    public int enemiesToSpawn = 10;
    
    public int enemiesDefeated = 0;
    private int currentEnemies = 0;
    private float timer = 0f;

    private GameObject currentBoss;

    public AIController _AIController;

    private void Start()
    {
        //GameObject playerWeaponObject = GameObject.FindGameObjectWithTag("PlayerWeapon");

       // PlayerWeapon playerWeapon = playerWeaponObject.GetComponent<PlayerWeapon>();
        
       // playerWeapon.setDamage(100f);
    }

    void Update()
    {
        // ตรวจสอบว่าจำนวนศัตรูที่เกิดอยู่ต่ำกว่าขีดจำกัดหรือไม่
        if (currentEnemies < maxEnemies && enemiesDefeated < totalEnemiesToDefeat)  // เอาไว้เช็คส่าตีครบหรือยัง
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }
        
        
        
    }
    public void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        if (currentBoss != null)
        {
            Destroy(currentBoss);
            currentBoss = null;
        }
    }
    
    public void SpawnBoss()
    {
        if (currentBoss == null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            currentBoss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("บอสปรากฏตัว!");
        }
    }
    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length); // เลือกจุดเกิดแบบสุ่ม
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        currentEnemies++;
    }
    public void SpawnEnemies(int level)
    {
        ClearEnemies();
        enemiesDefeated = 0;
      /*  for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }*/

        if (level == 5)
        {
            SpawnBoss();
        }
    }
    public void OnEnemyDefeated()
    {
        if (_AIController.isLastStage != true)
        {
            enemiesDefeated++;
        }
       
        currentEnemies--;

        if (enemiesDefeated >= totalEnemiesToDefeat)
        {
            UnlockNextLevel();
        }
    }

    void UnlockNextLevel()
    {
        if (_AIController.isLastStage != true)
        {
            enemiesDefeated = 0;
            totalEnemiesToDefeat += 1;
            _AIController.MoveToNextLevel();
            Debug.Log("เปิดด่านใหม่!");
            // ใส่ Logic ในการเปิดด่านใหม่ที่นี่ เช่น การโหลด Scene ใหม่
            // SceneManager.LoadScene("NextLevel");
        }
        else
        {
            
        }
       
    }
}
