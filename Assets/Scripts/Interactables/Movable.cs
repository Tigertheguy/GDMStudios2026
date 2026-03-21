using UnityEngine;

public class Movable : Interactable 
{
    public Transform holdPosition;
    private bool isHeld = false;
    private Rigidbody rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        print("found");
        if (holdPosition == null)
        {
            print("found");
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                print("found");
                holdPosition = player.transform.Find("HoldPos");
            }
        }
    }

    //We override the OnInteractAction method defined in Interactabke
    public override void OnInteractAction()
    {
        if (!isHeld) {
            PickUp();
        } else {
            Drop();
        }
    }

    private void PickUp()
    {
        Debug.Log("picked up");
        isHeld = true;
        rb.isKinematic = true; 
        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity; 
        Physics.SyncTransforms();

        // --- ADD THIS PART ---
        KeySpin spinner = GetComponent<KeySpin>();
        if (spinner != null)
        {
            spinner.enabled = false;
        }
    }

    private void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        transform.SetParent(null);
    }
}