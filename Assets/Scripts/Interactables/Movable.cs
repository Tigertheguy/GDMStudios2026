using UnityEngine;

public class Movable : Interactable 
{
    public Transform holdPosition;
    private bool isHeld = false;
    private Rigidbody rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();

        if (holdPosition == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
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
    }

    private void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        transform.SetParent(null);
    }
}