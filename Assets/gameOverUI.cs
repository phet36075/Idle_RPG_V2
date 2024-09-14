using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class gameOverUI : MonoBehaviour
{
    public string Stagename;
    public float timeRemaining = 10;
    public TextMeshProUGUI timeText;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timeText.text = timeRemaining.ToString("#");

        if (timeRemaining <= 0)
        {
            RetryStage();
        }
    }

  public void RetryStage()
    {
        SceneManager.LoadScene(Stagename);
    }
}
