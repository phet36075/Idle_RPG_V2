using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMeteorSpawn : MonoBehaviour
{
    public float WaitMeteorTime = 3;
    public float DestroyTime = 2.5f;
    public GameObject MeteorFxPrefab;
    
    
    
    public float radius = 1.15f;
    public LayerMask layerMask = -1;
    public Color gizmoColor = Color.red;

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Hit: " + hitCollider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForMeteorSpawn());
    }
    
    IEnumerator WaitForMeteorSpawn()
    {
        yield return new WaitForSeconds(WaitMeteorTime);
        MeteorFxPrefab.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        
        foreach (var hitCollider in hitColliders)
        {
            PlayerStats playerStats = hitCollider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(10);
            }
        }
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}
