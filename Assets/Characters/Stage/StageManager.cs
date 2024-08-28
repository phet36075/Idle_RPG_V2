using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage = 1;
    public int currentStagetxt;
    public int totalLevels = 5;
    public int stage = 0;

    public EnemySpawner _EnemySpawner;
    public EnvironmentManager environmentManager;
    public AIController _AIController;

    void Start()
    {
        //LoadLevel(currentStage);
    }

    public void LoadLevel(int level)
    {
        environmentManager.LoadEnvironment(level - 1); // เปลี่ยนสภาพแวดล้อม
        _EnemySpawner.SpawnEnemies(level); // สร้างศัตรู

        if (level == 5)
        {
            Debug.Log("ด่านที่ 5! จะต้องสู้กับบอสก่อน!");
            // เปลี่ยนเป้าหมาย AI ของผู้เล่นไปที่บอส
            GameObject boss = GameObject.FindWithTag("Boss");
            if (boss != null)
            {
                _AIController.SetTarget(boss.transform);
            }
        }
    }

    public void GoToNextLevel()
    {

        currentStage++;
        LoadLevel(currentStage);

    }

    public void OnBossDefeated()
    {
        if (currentStage == 5)
        {
            GoToNextLevel(); // ไปยังด่านถัดไปหลังจากบอสถูกกำจัด
        }
    }

    public void Update()
    {
        currentStagetxt =  stage+1;
    }
}
