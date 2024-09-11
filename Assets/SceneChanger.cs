using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public PlayerData PlayerData;

    private PlayerStats _playerStats;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // PlayerStats.Instance.OnSceneChanged();

           PlayerData.currentHealth = _playerStats.currentHealth;
            SceneManager.LoadScene("TestStageBoss");
        }
    }
}
