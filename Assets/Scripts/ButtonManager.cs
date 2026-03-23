using UnityEngine;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour
{

    public UnityEvent AllButtonsPressed;
    public UnityEvent NotAllButtonsPressed;

    private Button[] buttons;
    private bool allPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.OnChangeState += Check;
        }
        
    }

    private void Check()
    {
        bool currentlyAllPressed = true;
        // Check if all buttons are pressed
        foreach (Button button in buttons)
        {
            if (!button.isPressed)
            {
                currentlyAllPressed = false;
                break;
            }
        }

        if (currentlyAllPressed && !allPressed)
        {
            allPressed = true;
            AllButtonsPressed.Invoke();
            Debug.Log("All buttons pressed!");
        }
        else if (!currentlyAllPressed && allPressed)
        {
            allPressed = false;
            NotAllButtonsPressed.Invoke();
            Debug.Log("Not all buttons pressed!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
