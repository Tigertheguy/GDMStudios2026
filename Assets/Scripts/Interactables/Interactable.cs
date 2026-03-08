using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private string prompt = "E"; //Prompt message when hovering over an item

    public abstract void OnInteractAction(); //Subclasses will override this method

    public void BaseInteract() {
        Debug.Log("Interacted with: " + gameObject.name); //for test
        OnInteractAction();
    }
}
