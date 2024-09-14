using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public PlayerData PlayerData;

    private PlayerManager _playerManager;
    public string nextLevelName;


    private void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        PlayerData.currentHealth = _playerManager.currentHealth;
        SceneManager.LoadScene(nextLevelName);
    }
}
