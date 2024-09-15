using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class CongratulationUI : MonoBehaviour
{
    public float timeRemaining = 10;
    public TextMeshProUGUI timeText;

    public Transform NextDoorLocation;

    //public Animator playerAnimator;
   private AIController _aiController;

   private PlayerController _playerController;
    //public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        _aiController = FindObjectOfType<AIController>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timeText.text = timeRemaining.ToString("#");

        if (timeRemaining <= 0)
        {
           GoNextStage();
        }
    }

    public void GoNextStage()
    {
        
       _aiController.SetTarget(NextDoorLocation);
        gameObject.SetActive(false);
       
    }
}
