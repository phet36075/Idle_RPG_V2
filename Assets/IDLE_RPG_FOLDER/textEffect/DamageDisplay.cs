using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageDisplay : MonoBehaviour
{
    public GameObject damageTextPrefab, damageTextPrefabCritical;
    public Transform textLocation;
    public Vector3 randomOffsetRange = new Vector3(0.5f, 0.5f, 0.5f); // ระยะการสุ่มตำแหน่งสำหรับ X, Y, Z

    public string textToDisplay;
    //private Transform cam;

    public Vector3 vector;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    void Update()
    {
       /* if (textLocation != null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            textLocation.transform.LookAt(transform.position + cam.position);
            // textLocation.transform.LookAt(transform.position + cam.forward + vector);
        }*/
    }
    void LateUpdate()
    {
        /*if (textLocation != null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            textLocation.transform.LookAt(transform.position + cam.position);
           // textLocation.transform.LookAt(transform.position + cam.forward + vector);
        }*/
        
    }
    public void DisplayDamage(float damage)
    {
       /* GameObject damgeTextInstance = Instantiate(damageTextPrefab, textLocation.transform);
        damgeTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString("0"));*/
       
       // สร้าง instance ของ prefab ที่ตำแหน่งเริ่มต้น
       GameObject damageTextInstance = Instantiate(damageTextPrefab, textLocation.position, Quaternion.identity, textLocation);

       // สุ่มค่า offset สำหรับ X, Y และ Z
       float randomX = Random.Range(-randomOffsetRange.x, randomOffsetRange.x);
       float randomY = Random.Range(-randomOffsetRange.y, randomOffsetRange.y);
       float randomZ = Random.Range(-randomOffsetRange.z, randomOffsetRange.z);

       // ปรับตำแหน่งของ instance ด้วยค่า offset ที่สุ่มได้
       Vector3 randomOffset = new Vector3(randomX, randomY, randomZ);
       damageTextInstance.transform.position += randomOffset;

       // ตั้งค่าข้อความให้แสดงค่าความเสียหาย
       damageTextInstance.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString("0"));
       
       
       
       
       
       
    }
    public void DisplayDamageCritical(float damage)
    {
        GameObject damgeTextInstance = Instantiate(damageTextPrefabCritical, textLocation.transform);
      
       
        damgeTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString("0"));
    }

    // Update is called once per frame
   
}
