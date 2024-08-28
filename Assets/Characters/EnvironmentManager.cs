using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentManager : MonoBehaviour
{
    public Transform environmentContainer;
    public GameObject[] environmentPrefabs;
    public NavMeshSurface navMeshSurface;
    public EnemySpawner _EnemySpawner;
    public AIController _AIController;
   public int AllStage =5 ;
    private GameObject currentEnvironment;

    void Start()
    {
        navMeshSurface.BuildNavMesh();
        LoadEnvironment(0);
    }

    public void LoadEnvironment(int index)
    {
        if (currentEnvironment != null)
        {
            Destroy(currentEnvironment);
        }
        
        currentEnvironment = Instantiate(environmentPrefabs[index], environmentContainer.position, Quaternion.identity);
        currentEnvironment.transform.parent = environmentContainer;
        if (index == 4)
        {
            _EnemySpawner.SpawnBoss();
            GameObject boss = GameObject.FindWithTag("Boss");
            if (boss != null)
            {
                _AIController.SetTarget(boss.transform);
            }
        }
        navMeshSurface.BuildNavMesh(); // สร้าง NavMesh ใหม่หลังจากเปลี่ยนพื้น
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadEnvironment(1);
        }
    }
}
