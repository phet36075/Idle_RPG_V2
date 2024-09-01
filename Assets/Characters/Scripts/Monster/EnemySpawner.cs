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

    void Start()
    {
        // เริ่มการเรียกฟังก์ชัน SpawnEnemy ซ้ำๆ ทุก spawnInterval วินาที
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // สุ่มตำแหน่งภายในวงกลม
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(randomPos.x, 0, randomPos.y) + transform.position;

        // สร้างศัตรูที่ตำแหน่งที่สุ่มได้
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
   
}
