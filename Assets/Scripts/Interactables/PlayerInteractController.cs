using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractController : MonoBehaviour
{
    public float radius = 2f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI promptText;

    public void OnInteractTriggered(InputAction.CallbackContext context) 
    {
        if (context.performed) 
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

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, interactableLayer);

        if (colliders.Length > 0)
        {
            if (colliders[0].TryGetComponent(out Interactable interactable))
            {
                promptText.text = "E";
                return;
            }
        }
        
        promptText.text = "";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
