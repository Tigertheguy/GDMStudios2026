using UnityEngine;
using UnityEngine.Events;

public class Consumable : Interactable 
{
    public UnityEvent OnConsume;

    public override void OnInteractAction()
    {
        Debug.Log("Consumed the following item: " + gameObject.name);
        OnConsume.Invoke(); // Use methods in Unity Events
        Destroy(gameObject); //Bye bye object
    }

    public void TestFunction()
    {
        print("Consumed");
    }
}