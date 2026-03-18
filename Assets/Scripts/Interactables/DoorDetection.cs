using UnityEngine;

public class DoorDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided has KeyScript component
        if (other.gameObject.GetComponent<KeyScript>() != null)
        {
            Debug.Log("Key detected! You can now open the door.");


            Collider[] allColliders = GetComponents<Collider>();
            foreach (Collider col in allColliders)
            {
                col.enabled = false;
            }

            if (TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
        }
    }
}
