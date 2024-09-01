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
        
    }

    public void Update()
    {
        currentStagetxt =  stage+1;
    }
}
