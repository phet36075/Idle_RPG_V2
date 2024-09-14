using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMotionEffect : MonoBehaviour
{
    public float frameDuration = 0.1f; // ระยะเวลาของแต่ละเฟรม
    public int frameSkip = 2; // จำนวนเฟรมที่ข้ามในแต่ละครั้ง
    
    private bool isStopMotionActive = false;
    private WaitForSeconds waitDuration;

    void Start()
    {
        waitDuration = new WaitForSeconds(frameDuration);
    }

    // เรียกฟังก์ชันนี้เมื่อต้องการเริ่ม stop motion effect
    public void StartStopMotion()
    {
        if (!isStopMotionActive)
        {
            isStopMotionActive = true;
            StartCoroutine(StopMotionCoroutine());
        }
    }

    // เรียกฟังก์ชันนี้เมื่อต้องการหยุด stop motion effect
    public void StopStopMotion()
    {
        isStopMotionActive = false;
    }

    IEnumerator StopMotionCoroutine()
    {
        Time.timeScale = 1f; // รีเซ็ต timeScale

        while (isStopMotionActive)
        {
            // หยุดเวลาชั่วคราว
            Time.timeScale = 0f;
            yield return waitDuration;

            // เร่งเวลาเพื่อข้ามไปยังเฟรมถัดไป
            Time.timeScale = 9999f;
            yield return new WaitForSecondsRealtime(0.001f);

            // ข้ามเฟรมตามจำนวนที่กำหนด
            for (int i = 0; i < frameSkip; i++)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        Time.timeScale = 1f; // คืนค่า timeScale เป็นปกติ
    }
}
