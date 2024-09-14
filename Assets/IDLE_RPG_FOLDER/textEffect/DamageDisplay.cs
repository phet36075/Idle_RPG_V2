using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageDisplay : MonoBehaviour
{
    public GameObject damageTextPrefab, damageTextPrefabCritical, textLocation;
    
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
        GameObject damgeTextInstance = Instantiate(damageTextPrefab, textLocation.transform);
       // cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
       
        damgeTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString("0"));
    }
    public void DisplayDamageCritical(float damage)
    {
        GameObject damgeTextInstance = Instantiate(damageTextPrefabCritical, textLocation.transform);
        // cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
       
        damgeTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString("0"));
    }

    // Update is called once per frame
   
}
