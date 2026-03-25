using UnityEngine;

public class KeySpin : MonoBehaviour
{ 
    [SerializeField] public AK.Wwise.Event _startKeyPickup;

    public float spinSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object walking into the key is the Player
        if (other.CompareTag("Player"))
        {
            // Disable this specific script so Update stops running
            this.enabled = false;

            Debug.Log("The key has stopped spinning!");
            
            // If you want the key to follow the player, add that here:
            // transform.SetParent(other.transform);

            _startKeyPickup.Post(gameObject);
            //add key audio
        }
    }
}
