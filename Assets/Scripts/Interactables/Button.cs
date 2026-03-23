using System;
using Unity.AppUI.UI;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool isPressed = false;
    public event Action OnChangeState;
    public int totalEntities = 0;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pushable") || other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
             totalEntities++;
             CheckState();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable") || other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            totalEntities--;
            CheckState();
        }
    }

    private void CheckState()
    {
        bool shouldBePressed = totalEntities > 0;
        if (shouldBePressed != isPressed)
        {
            isPressed = shouldBePressed;
            OnChangeState?.Invoke();
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
