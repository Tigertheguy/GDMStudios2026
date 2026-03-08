using Unity.VisualScripting;
using UnityEngine;

public class SanityDrainer : MonoBehaviour
{
    [SerializeField] private float _sanityDrainAmount = 10f;
    [SerializeField] private float _drainRadius = 20f;

    private SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = _drainRadius;
        sphereCollider.isTrigger = true;

        if (sphereCollider == null)
    {
        Debug.LogError("No collider found");
        return;
    }
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player in zone");
        if (other.CompareTag("Player"))
        {
            
            PlayerSanity player = other.GetComponent<PlayerSanity>();
            if(player != null)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                float proximityIntensity = 1f - Mathf.Clamp01(distance / _drainRadius);
                player.DrainSanity(_sanityDrainAmount * proximityIntensity);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
