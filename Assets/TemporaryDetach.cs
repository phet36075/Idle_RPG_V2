using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDetach : MonoBehaviour
{
    public Transform originalParent; // This will store the original parent
    private Transform originalPos;

    void Start()
    {
        // Save the original parent at the start
        originalParent = transform.parent;
        originalPos.transform.position = transform.position;
        originalPos.transform.rotation = transform.rotation;
        
      
    }

    public void DetachTemporarily(float delay)
    {
        // Detach the object from its parent
        transform.parent = null;
         Debug.Log("Detach");
        // Call Reattach after the delay
        Invoke("Reattach", delay);
    }

    void Reattach()
    {
        // Reattach the object to its original parent
        transform.parent = originalParent;
        transform.position = originalPos.transform.position;
        transform.rotation = originalPos.transform.rotation;
        
    }
}
