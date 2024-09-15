using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitSound,hitCritSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayHitSound()
    {
        
        audioSource.PlayOneShot(hitSound);
        
    }

    public void PlayHitCritSound()
    {
        
        audioSource.PlayOneShot(hitCritSound);
    }
}
