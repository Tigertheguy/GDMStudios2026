using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractController : MonoBehaviour
{
   [SerializeField] public float radius = 2f;
   [SerializeField] public LayerMask interactableLayer;

    //When E is pressed basically
    public void OnInteract(InputValue value) 
    {
        if (value.isPressed) 
        {
            CheckForItems();
        }
    }

    private void CheckForItems()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, interactableLayer);

        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out Interactable interactable)) 
            { //We try and get the interactable script
                interactable.BaseInteract();
                break; //We only interact with one item
            }
        }
    }
}
