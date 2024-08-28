using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI _txtPlayerHealth;
    public TextMeshProUGUI _txtStage;
    //public AIController _AIController;
   
    public TextMeshProUGUI _txtTotalToDefeated;
    public TextMeshProUGUI _txtEnemiesDefeated;
    public StageManager _StageManager;

    public EnemySpawner _EnemySpawner;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        _txtPlayerHealth.text = playerHealth.playerHealth.ToString();
        _txtStage.text = _StageManager.currentStagetxt.ToString();
        _txtEnemiesDefeated.text = _EnemySpawner.enemiesDefeated.ToString();
        _txtTotalToDefeated.text = _EnemySpawner.totalEnemiesToDefeat.ToString();
    }
}
