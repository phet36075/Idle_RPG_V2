using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FreeLookCameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public Transform character;
    public string mouseXInputName = "Mouse X";
    public string mouseYInputName = "Mouse Y";
    public string mouseScrollInputName = "Mouse ScrollWheel";
    public float zoomSpeed = 10f;
    public float minZoom = 1f;
    public float maxZoom = 50f;
    public float lookAtCameraDistance = 3f; // ระยะที่ตัวละครจะหันหน้ามาทางกล้อง
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) // คลิกขวาเพื่อหมุนกล้อง
        {
            freeLookCamera.m_XAxis.m_InputAxisName = mouseXInputName;
            freeLookCamera.m_YAxis.m_InputAxisName = mouseYInputName;
        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }

        // ซูมกล้องด้วย Scroll Wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        freeLookCamera.m_Lens.FieldOfView -= scrollInput * zoomSpeed;
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView, minZoom, maxZoom);
        
        if (freeLookCamera.m_Lens.FieldOfView <= lookAtCameraDistance)
        {
            LookAtCamera();
        }
    }
    
    void LookAtCamera()
    {
        Vector3 directionToCamera = freeLookCamera.transform.position - character.position;
        directionToCamera.y = 0; // ล็อคแกน Y เพื่อไม่ให้ตัวละครเงยหรือก้ม
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        character.rotation = Quaternion.Slerp(character.rotation, lookRotation, Time.deltaTime * 5f); // ปรับค่าเพื่อควบคุมความเร็วในการหันหน้า
    }
    
}
