using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab ของศัตรู
    public Transform[] spawnPoints; // จุดที่จะใช้สุ่มเกิดศัตรู
    public int maxEnemies = 10; // จำนวนศัตรูสูงสุดที่สามารถเกิดได้พร้อมกัน
    public int totalEnemiesToDefeat = 20; // จำนวนศัตรูทั้งหมดที่ต้องกำจัดเพื่อเปิดด่านใหม่
    public float spawnInterval = 2f; // เวลาระหว่างการเกิดแต่ละตัว

    private int enemiesDefeated = 0;
    private int currentEnemies = 0;
    private float timer = 0f;


    private void Start()
    {
        GameObject playerWeaponObject = GameObject.FindGameObjectWithTag("PlayerWeapon");

        PlayerWeapon playerWeapon = playerWeaponObject.GetComponent<PlayerWeapon>();
        
        playerWeapon.setDamage(100f);
    }

    void Update()
    {
        // ตรวจสอบว่าจำนวนศัตรูที่เกิดอยู่ต่ำกว่าขีดจำกัดหรือไม่
        if (currentEnemies < maxEnemies /*&& enemiesDefeated < totalEnemiesToDefeat*/)  // เอาไว้เช็คส่าตีครบหรือยัง
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length); // เลือกจุดเกิดแบบสุ่ม
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        currentEnemies++;
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        currentEnemies--;

        if (enemiesDefeated >= totalEnemiesToDefeat)
        {
            UnlockNextLevel();
        }
    }

    void UnlockNextLevel()
    {
        Debug.Log("เปิดด่านใหม่!");
        // ใส่ Logic ในการเปิดด่านใหม่ที่นี่ เช่น การโหลด Scene ใหม่
        // SceneManager.LoadScene("NextLevel");
    }
}
