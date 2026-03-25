using System.Collections; // Required for Coroutines
using UnityEngine;

public class DoorDetection : MonoBehaviour
{
    
    [SerializeField] private AK.Wwise.Event _startDoorOpen;
    
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<KeyScript>() != null)
        {
            Destroy(other.gameObject);
            StartCoroutine(OpenDoors());
            GetComponent<Collider>().enabled = false;
            
            Debug.Log("Door opened");

            _startDoorOpen.Post(gameObject);

            //key in door sound
        }
    }

    IEnumerator OpenDoors()
    {
    float elapsed = 0;

    Quaternion leftStart = leftDoor.transform.localRotation;
    Quaternion rightStart = rightDoor.transform.localRotation;

    Quaternion leftTarget = leftStart * Quaternion.Euler(0, -90, 0);
    Quaternion rightTarget = rightStart * Quaternion.Euler(0, 90, 0);

    while (elapsed < 1f)
    {
        elapsed += Time.deltaTime * openSpeed;
        leftDoor.transform.localRotation = Quaternion.Slerp(leftStart, leftTarget, elapsed);
        rightDoor.transform.localRotation = Quaternion.Slerp(rightStart, rightTarget, elapsed);
        
        yield return null;
    }
    leftDoor.transform.localRotation = leftTarget;
    rightDoor.transform.localRotation = rightTarget;
    }
}