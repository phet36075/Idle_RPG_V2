using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AIController aiController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // ปุ่มเพื่อสลับระหว่างผู้เล่นและ AI
        {
            ToggleAI();
        }
    }

    public void ToggleAI()
    {
        bool isAIEnabled = aiController.enabled;
        aiController.enabled = !isAIEnabled;
        if (!isAIEnabled) // ถ้ากำลังจะเปลี่ยนไป AI Mode
        {
            aiController.FindNearestEnemy();
        }
        // ปิด/เปิดการควบคุมผู้เล่น (เช่น Input ของ Character Controller)
        GetComponent<CharacterController>().enabled = isAIEnabled;
    }
}
